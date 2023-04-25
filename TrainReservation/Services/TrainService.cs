using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainReservation.Models;

namespace TrainReservation.Services
{
    public class TrainService
    {

        public static ResponseModel VagonaAta(RequestModel request)
        {
            ResponseModel response = SetResponse(request);
            return response;
        }

        private static ResponseModel SetResponse(RequestModel request)
        {
            ResponseModel response = new ResponseModel();
            response = SetDefaultResponse();
            
            try
            {
                if (request != null && request.RezervasyonYapilacakKisiSayisi != 0)
                {
                    if (request.Tren != null && request.Tren.Vagonlar != null && request.Tren.Vagonlar.Count > 0)
                    {
                        List<WagonModel> kullanılabilirVagonListesi = request.Tren.Vagonlar.Where(wagon => wagon.DoluKoltukAdet < wagon.Kapasite * 0.7).ToList();
                        Dictionary<string, int> vagonKoltuk = GetBosVagon(kullanılabilirVagonListesi);
                        KoltukAyarla(vagonKoltuk, request.RezervasyonYapilacakKisiSayisi, request.KisilerFarkliVagonlaraYerlestirilebilir, response);
                    }
                }
            }
            catch (Exception ex)
            {
                //response.HataMesaj = ex.Message;
            }

            return response;
        }

        private static ResponseModel SetDefaultResponse()
        {
            ResponseModel response = new ResponseModel();
            response.YerlesimAyrinti = new List<PlacementDetailModel>();
            response.RezervasyonYapilabilir = true;
            return response;
        }

        private static List<PlacementDetailModel> Yerleştir(Dictionary<string, int> vagonKoltuk, int yerlesecekKisiSayisi)
        {
            List<PlacementDetailModel> yerlesimListesi = new List<PlacementDetailModel> { };
            PlacementDetailModel yerlesim = null;

            Dictionary<string, int> vagonKoltukCopy = vagonKoltuk.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

            foreach (var vagon in vagonKoltukCopy)
            {
                yerlesim = new PlacementDetailModel();
                if (yerlesecekKisiSayisi <= 0)
                {
                    break;
                }

                int yerlesimSayi = 0;
                int kalanBosYer = 0;

                if (vagon.Value < yerlesecekKisiSayisi)
                {
                    yerlesimSayi = vagon.Value;
                    kalanBosYer = 0;
                }
                else
                {
                    yerlesimSayi = yerlesecekKisiSayisi;
                    kalanBosYer = vagon.Value - yerlesecekKisiSayisi;
                }

                yerlesim.VagonAdi = vagon.Key;
                yerlesim.KisiSayisi = yerlesimSayi;
                yerlesecekKisiSayisi -= yerlesimSayi;
                vagonKoltuk[vagon.Key] = kalanBosYer;
                yerlesimListesi.Add(yerlesim);
            }

            return yerlesimListesi;
        }

        private static void KoltukAyarla(Dictionary<string, int> vagonKoltuk, int rezervasyonYapilacakKisiSayisi, bool kisilerFarkliVagonlaraYerlestirilebilir, ResponseModel response)
        {
            int toplamBoşKoltuk = vagonKoltuk.Sum(vagon => vagon.Value);

            if (toplamBoşKoltuk >= rezervasyonYapilacakKisiSayisi)
            {
                if (kisilerFarkliVagonlaraYerlestirilebilir)
                {
                    List<PlacementDetailModel> yerlesimListesi = Yerleştir(vagonKoltuk, rezervasyonYapilacakKisiSayisi);
                    response.YerlesimAyrinti = yerlesimListesi;
                    response.RezervasyonYapilabilir = true;
                }
                else
                {
                    var yerlesecekVagon = vagonKoltuk.FirstOrDefault(vagon => vagon.Value >= rezervasyonYapilacakKisiSayisi);
                    vagonKoltuk.Clear();
                    if (!string.IsNullOrEmpty(yerlesecekVagon.Key) || yerlesecekVagon.Value > 0)
                    {
                        vagonKoltuk.Add(yerlesecekVagon.Key, yerlesecekVagon.Value);
                        if (yerlesecekVagon.Value > 0)
                        {
                            List<PlacementDetailModel> yerlesimListesi = Yerleştir(vagonKoltuk, rezervasyonYapilacakKisiSayisi);
                            response.YerlesimAyrinti = yerlesimListesi;
                            response.RezervasyonYapilabilir = true;
                        }
                    }
                }
            }
        }

        private static Dictionary<string, int> GetBosVagon(List<WagonModel> kullanılabilirVagonListesi)
        {
            Dictionary<string, int> vagonKoltuk = new Dictionary<string, int>();

            kullanılabilirVagonListesi.ForEach(vagon => { vagonKoltuk.Add(vagon.Ad, (int)Math.Floor(vagon.Kapasite * 0.7 - vagon.DoluKoltukAdet)); });

            return vagonKoltuk;
        }

    }
}