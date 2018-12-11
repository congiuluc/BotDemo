using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using static XmasDevGiftAssistantDemo.Shopping.Enums;

namespace XmasDevGiftAssistantDemo.Shopping
{
    public class ShoppingHelperIT: IShoppingHelper
    {
        private readonly ILogger _logger;

        public ShoppingHelperIT(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ShoppingHelperIT>();
        }

        public Dictionary<int, string> GetSearchIndexDictionary()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(typeof(SearchIndexIT)))
            {
                result.Add(value, GetSearchIndexDesc(value));
            }

            return result;
        }


        public Dictionary<int, string> GetSearchIndexDictionary(int age)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<int> categories;
            if (age >= 0 && age < 5)
            {
                categories = new List<int>()
                {
                // (int)SearchIndexIT.TutteLeCategorie,
                 (int)SearchIndexIT.Abbigliamento,
                 (int)SearchIndexIT.BuoniRegalo,
                 (int)SearchIndexIT.GiochiEGiocattoli,
                 (int)SearchIndexIT.PrimaInfanzia,
                };
            }
            else if (age >= 5 && age < 10)
            {
                categories = new List<int>()
                {
                // (int)SearchIndexIT.TutteLeCategorie,
                 (int)SearchIndexIT.Abbigliamento,
                 (int)SearchIndexIT.BuoniRegalo,
                 (int)SearchIndexIT.GiochiEGiocattoli,
                 //(int)SearchIndexIT.Videogiochi,
                 //(int)SearchIndexIT.Libri,
                 //(int)SearchIndexIT.LibriInAltreLingue,
                 //(int)SearchIndexIT.AppEGiochi,
                };
            }
            else if (age >= 10 && age < 15)
            {
                categories = new List<int>()
                {
                //(int)SearchIndexIT.TutteLeCategorie,
                 (int)SearchIndexIT.Abbigliamento,
                 (int)SearchIndexIT.BuoniRegalo,
                 (int)SearchIndexIT.GiochiEGiocattoli,
                 (int)SearchIndexIT.Videogiochi,
                 (int)SearchIndexIT.Libri,
                 //(int)SearchIndexIT.LibriInAltreLingue,
                 (int)SearchIndexIT.AppEGiochi,
                 //(int)SearchIndexIT.Elettronica,
                 (int)SearchIndexIT.KindleStore,
                 (int)SearchIndexIT.MusicaDigitale,
                 (int)SearchIndexIT.SportETempoLibero,
                 //(int)SearchIndexIT.Informatica,
                 //(int)SearchIndexIT.StrumentiMusicaliEDJ,

                };

            }
            else if (age >= 15 && age < 20)
            {
                categories = new List<int>()
                {
                 //(int)SearchIndexIT.TutteLeCategorie,
                 (int)SearchIndexIT.Abbigliamento,
                 (int)SearchIndexIT.BuoniRegalo,
                 (int)SearchIndexIT.GiochiEGiocattoli,
                 (int)SearchIndexIT.Gioielli,
                 (int)SearchIndexIT.Videogiochi,
                 //(int)SearchIndexIT.Libri,
                 //(int)SearchIndexIT.LibriInAltreLingue,
                 (int)SearchIndexIT.AppEGiochi,
                 //(int)SearchIndexIT.Elettronica,
                 //(int)SearchIndexIT.KindleStore,
                 (int)SearchIndexIT.MusicaDigitale,
                 (int)SearchIndexIT.SportETempoLibero,
                 (int)SearchIndexIT.Informatica,
                 (int)SearchIndexIT.AutoEMoto,
                 (int)SearchIndexIT.Bellezza,
                 //(int)SearchIndexIT.IndustriaEScienza,
                 //(int)SearchIndexIT.StrumentiMusicaliEDJ,

                };
            }
            else if (age >= 20 && age < 30)
            {
                categories = new List<int>()
                {
                 //(int)SearchIndexIT.TutteLeCategorie,
                 (int)SearchIndexIT.Abbigliamento,
                 (int)SearchIndexIT.BuoniRegalo,
                 (int)SearchIndexIT.Videogiochi,
                 (int)SearchIndexIT.Gioielli,
                 //(int)SearchIndexIT.Libri,
                 //(int)SearchIndexIT.LibriInAltreLingue,
                 //(int)SearchIndexIT.AppEGiochi,
                 (int)SearchIndexIT.Elettronica,
                 (int)SearchIndexIT.KindleStore,
                 (int)SearchIndexIT.MusicaDigitale,
                 (int)SearchIndexIT.SportETempoLibero,
                 (int)SearchIndexIT.Informatica,
                 (int)SearchIndexIT.AutoEMoto,
                 (int)SearchIndexIT.Bellezza,
                 //(int)SearchIndexIT.IndustriaEScienza,
                 (int)SearchIndexIT.FilmETv,
                 //(int)SearchIndexIT.CancelleriaEProdottiPerUfficio,
                 //(int)SearchIndexIT.CDEVinili,
                 //(int)SearchIndexIT.CasaECucina,
                // (int)SearchIndexIT.AlimentariECuraDellaCasa,
                //(int)SearchIndexIT.StrumentiMusicaliEDJ,
};
            }
            else if (age >= 30 && age < 40)
            {
                categories = new List<int>()
                {
                // (int)SearchIndexIT.TutteLeCategorie,
                 (int)SearchIndexIT.Abbigliamento,
                 (int)SearchIndexIT.BuoniRegalo,
                 (int)SearchIndexIT.Libri,
                 (int)SearchIndexIT.LibriInAltreLingue,
                 (int)SearchIndexIT.AppEGiochi,
                 (int)SearchIndexIT.Elettronica,
                 (int)SearchIndexIT.KindleStore,
                 (int)SearchIndexIT.MusicaDigitale,
                 (int)SearchIndexIT.SportETempoLibero,
                 (int)SearchIndexIT.Informatica,
                 (int)SearchIndexIT.AutoEMoto,
                 (int)SearchIndexIT.Bellezza,
                 (int)SearchIndexIT.IndustriaEScienza,
                 (int)SearchIndexIT.FilmETv,
                 (int)SearchIndexIT.CancelleriaEProdottiPerUfficio,
                 (int)SearchIndexIT.CDEVinili,
                 (int)SearchIndexIT.CasaECucina,
                 (int)SearchIndexIT.AlimentariECuraDellaCasa,
                 (int)SearchIndexIT.FaiDaTe,
                 (int)SearchIndexIT.Gioielli,
                 (int)SearchIndexIT.Handmade,
                 (int)SearchIndexIT.StrumentiMusicaliEDJ,
                 (int)SearchIndexIT.Videogiochi,

                };

            }
            else
            {
                categories = new List<int>()
                {
                 //(int)SearchIndexIT.TutteLeCategorie,
                 (int)SearchIndexIT.AppEGiochi,
                 (int)SearchIndexIT.Abbigliamento,
                 (int)SearchIndexIT.AlimentariECuraDellaCasa,
                 (int)SearchIndexIT.AutoEMoto,
                 (int)SearchIndexIT.CancelleriaEProdottiPerUfficio,
                 (int)SearchIndexIT.CasaECucina,
                 (int)SearchIndexIT.CDEVinili,
                 (int)SearchIndexIT.Bellezza,
                 (int)SearchIndexIT.BuoniRegalo,
                 (int)SearchIndexIT.Elettronica,
                 (int)SearchIndexIT.FaiDaTe,
                 (int)SearchIndexIT.FilmETv,
                 (int)SearchIndexIT.Gioielli,
                 (int)SearchIndexIT.Handmade,
                 (int)SearchIndexIT.IndustriaEScienza,
                 (int)SearchIndexIT.Informatica,
                 (int)SearchIndexIT.KindleStore,
                 (int)SearchIndexIT.Libri,
                 (int)SearchIndexIT.LibriInAltreLingue,
                 (int)SearchIndexIT.MusicaDigitale,
                 (int)SearchIndexIT.SportETempoLibero,
                 (int)SearchIndexIT.StrumentiMusicaliEDJ,

                };
            }

            foreach (int value in categories)
            {
                result.Add(value, GetSearchIndexDesc(value));
            }

            return result;
        }

        public Dictionary<int, string> GetSearchIndexDictionary(int age, Gender gender)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
           
            switch (gender)
            {
                case Gender.female:
                    result = GetSearchIndexDictionary(age);
                    result.Remove((int)SearchIndexIT.Informatica);
                    result.Remove((int)SearchIndexIT.FaiDaTe);
                    result.Remove((int)SearchIndexIT.CDEVinili);
                    result.Remove((int)SearchIndexIT.AutoEMoto);
                    result.Remove((int)SearchIndexIT.IndustriaEScienza);
                    result.Remove((int)SearchIndexIT.CancelleriaEProdottiPerUfficio);
                    result.Remove((int)SearchIndexIT.StrumentiMusicaliEDJ);

                    break;
                case Gender.male:
                    result = GetSearchIndexDictionary(age);
                    result.Remove((int)SearchIndexIT.Gioielli);
                    result.Remove((int)SearchIndexIT.AlimentariECuraDellaCasa);
                    result.Remove((int)SearchIndexIT.CasaECucina);
                    result.Remove((int)SearchIndexIT.ScarpeEBorse);
                    result.Remove((int)SearchIndexIT.Valigeria);
                    result.Remove((int)SearchIndexIT.IndustriaEScienza);



                    break;
                default:
                    break;
            }


        
            return result;
        }


        public string GetSearchIndexDesc(int searchIndex)
        {
            SearchIndexIT category = (SearchIndexIT)searchIndex;

            string result = string.Empty;
            switch (category)
            {
                case SearchIndexIT.TutteLeCategorie:
                    result = "Tutte le categorie";
                    break;
                case SearchIndexIT.Abbigliamento:
                    result = "Abbigliamento";
                    break;
                case SearchIndexIT.AutoEMoto:
                    result = "Auto e moto";
                    break;
                case SearchIndexIT.PrimaInfanzia:
                    result = "Prima infanzia";
                    break;
                case SearchIndexIT.Bellezza:
                    result = "Bellezza";
                    break;
                case SearchIndexIT.Libri:
                    result = "Libri";
                    break;
                case SearchIndexIT.FilmETv:
                    result = "Film e TV";
                    break;
                case SearchIndexIT.Elettronica:
                    result = "Elettronica";
                    break;
                case SearchIndexIT.LibriInAltreLingue:
                    result = "Libri altre lingue";
                    break;
                case SearchIndexIT.GiardinoEGiardinaggio:
                    result = "Giardino e giardinaggio";
                    break;
                case SearchIndexIT.BuoniRegalo:
                    result = "Buoni regalo";
                    break;
                case SearchIndexIT.AlimentariECuraDellaCasa:
                    result = "Alimentari e cura della casa";
                    break;
                case SearchIndexIT.Handmade:
                    result = "Oggetti fati a mano";
                    break;
                case SearchIndexIT.CuraDellaPersona:
                    result = "Cura della persona";
                    break;
                case SearchIndexIT.IndustriaEScienza:
                    result = "Industria e scienza";
                    break;
                case SearchIndexIT.Gioielli:
                    result = "Gioielli";
                    break;
                case SearchIndexIT.KindleStore:
                    result = "Kindle Store";
                    break;
                case SearchIndexIT.CasaECucina:
                    result = "Casa e cucina";
                    break;
                case SearchIndexIT.Valigeria:
                    result = "Valigeria";
                    break;
                case SearchIndexIT.AppEGiochi:
                    result = "App e giochi";
                    break;
                case SearchIndexIT.MusicaDigitale:
                    result = "Musica digitale";
                    break;
                case SearchIndexIT.CDEVinili:
                    result = "Cd e venili";
                    break;
                case SearchIndexIT.StrumentiMusicaliEDJ:
                    result = "Strumenti musicali DJ";
                    break;
                case SearchIndexIT.CancelleriaEProdottiPerUfficio:
                    result = "Cancelleria e prodotti per ufficio";
                    break;
                case SearchIndexIT.Informatica:
                    result = "Informatica";
                    break;
                case SearchIndexIT.Software:
                    result = "Software";
                    break;
                case SearchIndexIT.SportETempoLibero:
                    result = "Sport e tempo libero";
                    break;
                case SearchIndexIT.FaiDaTe:
                    result = "Fai da te";
                    break;
                case SearchIndexIT.GiochiEGiocattoli:
                    result = "Giochi e giocattoli";
                    break;
                case SearchIndexIT.Videogiochi:
                    result = "Videogiochi";
                    break;
                case SearchIndexIT.Orologi:
                    result = "Orologi";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        public long GetRootIdBySearchIndex(int searchIndex)
        {
            SearchIndexIT category = (SearchIndexIT)searchIndex;

            long result = 0;
            switch (category)
            {
                case SearchIndexIT.TutteLeCategorie:
                    result = 0;
                    break;
                case SearchIndexIT.Abbigliamento:
                    result = 2844434031;
                    break;
                case SearchIndexIT.AutoEMoto:
                    result = 1571281031;
                    break;
                case SearchIndexIT.PrimaInfanzia:
                    result = 1571287031;
                    break;
                case SearchIndexIT.Bellezza:
                    result = 6198083031;
                    break;
                case SearchIndexIT.Libri:
                    result = 411664031;
                    break;
                case SearchIndexIT.FilmETv:
                    result = 412607031;
                    break;
                case SearchIndexIT.Elettronica:
                    result = 412610031;
                    break;
                case SearchIndexIT.LibriInAltreLingue:
                    result = 433843031;
                    break;
                case SearchIndexIT.GiardinoEGiardinaggio:
                    result = 635017031;
                    break;
                case SearchIndexIT.BuoniRegalo:
                    result = 3557018031;
                    break;
                case SearchIndexIT.AlimentariECuraDellaCasa:
                    result = 6198093031;
                    break;
                case SearchIndexIT.Handmade:
                    result = 9699426031;
                    break;
                case SearchIndexIT.CuraDellaPersona:
                    result = 1571290031;
                    break;
                case SearchIndexIT.IndustriaEScienza:
                    result = 5866069031;
                    break;
                case SearchIndexIT.Gioielli:
                    result = 2454164031;
                    break;
                case SearchIndexIT.KindleStore:
                    result = 1331141031;
                    break;
                case SearchIndexIT.CasaECucina:
                    result = 524016031;
                    break;
                case SearchIndexIT.Valigeria:
                    result = 2454149031;
                    break;
                case SearchIndexIT.AppEGiochi:
                    result = 1661661031;
                    break;
                case SearchIndexIT.MusicaDigitale:
                    result = 1748204031;
                    break;
                case SearchIndexIT.CDEVinili:
                    result = 412601031;
                    break;
                case SearchIndexIT.StrumentiMusicaliEDJ:
                    result = 3628630031;
                    break;
                case SearchIndexIT.CancelleriaEProdottiPerUfficio:
                    result = 3606311031;
                    break;
                case SearchIndexIT.Informatica:
                    result = 425917031;
                    break;
                case SearchIndexIT.Software:
                    result = 412613031;
                    break;
                case SearchIndexIT.SportETempoLibero:
                    result = 524013031;
                    break;
                case SearchIndexIT.FaiDaTe:
                    result = 2454161031;
                    break;
                case SearchIndexIT.GiochiEGiocattoli:
                    result = 523998031;
                    break;
                case SearchIndexIT.Videogiochi:
                    result = 412604031;
                    break;
                case SearchIndexIT.Orologi:
                    result = 524010031;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }
    }
}
