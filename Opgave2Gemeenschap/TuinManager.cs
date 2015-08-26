using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Collections.ObjectModel;


namespace Gemeenschap
{
    public class TuinManager
    {
        public void LeverancierToevoegen(Leverancier eenLeverancier)
        {
            var manager = new TuinDBManager();

            using (var conTuin = manager.GetConnection())
            {
                using (var comToevoegen = conTuin.CreateCommand())
                {
                    comToevoegen.CommandType = CommandType.StoredProcedure;
                    comToevoegen.CommandText = "LeverancierToevoegen";

                    var parNaam = comToevoegen.CreateParameter();
                    parNaam.ParameterName = "@Naam";
                    parNaam.Value = eenLeverancier.Naam;
                    comToevoegen.Parameters.Add(parNaam);
                    var parAdres = comToevoegen.CreateParameter();
                    parAdres.ParameterName = "@Adres";
                    parAdres.Value = eenLeverancier.Adres;
                    comToevoegen.Parameters.Add(parAdres);
                    var parPostNr = comToevoegen.CreateParameter();
                    parPostNr.ParameterName = "@PostNr";
                    parPostNr.Value = eenLeverancier.PostNr;
                    comToevoegen.Parameters.Add(parPostNr);
                    var parWoonplaats = comToevoegen.CreateParameter();
                    parWoonplaats.ParameterName = "@Woonplaats";
                    parWoonplaats.Value = eenLeverancier.Woonplaats;
                    comToevoegen.Parameters.Add(parWoonplaats);

                    using (var comAutoNumber = conTuin.CreateCommand())
                    {
                        comAutoNumber.CommandType = CommandType.StoredProcedure;
                        comAutoNumber.CommandText = "AutoNumberOphalen";

                        conTuin.Open();
                        comToevoegen.ExecuteNonQuery();
                        eenLeverancier.LevNr = Convert.ToInt32(comAutoNumber.ExecuteScalar());
                      
                    }
                }
            }
        }
        public int Eindejaarskorting()
        {
            var manager = new TuinDBManager();
            using (var conTuin = manager.GetConnection())
            {
                using (var comToevoegen = conTuin.CreateCommand())
                {
                    comToevoegen.CommandType = CommandType.StoredProcedure;
                    comToevoegen.CommandText = "EindejaarsKorting";
                    conTuin.Open();
                    return comToevoegen.ExecuteNonQuery();
                }
            }
        }

        public Decimal Gemiddelde1SoortBerekenen(string soort)
        {
            var dbManager = new TuinDBManager();

            using(var conTuin = dbManager.GetConnection())
            {
                using(var comBerekenen = conTuin.CreateCommand())
                {
                    comBerekenen.CommandType = CommandType.StoredProcedure;
                    comBerekenen.CommandText = "GemiddeldeBerekenen";

                    var parSoort = comBerekenen.CreateParameter();
                    parSoort.ParameterName = "@Soort";
                    parSoort.Value = soort;
                    comBerekenen.Parameters.Add(parSoort);

                    conTuin.Open();
                    Object result = comBerekenen.ExecuteScalar();
                    if (result == null || result.ToString() == "")
                    {
                        throw new Exception("Soort bestaat niet");
                    }
                    else
                        return (Decimal)result;
                }
            }
        }
        public void VervangLeverancier(int oudLevNr, int nieuwLevNr)
        {
            var dbManager = new TuinDBManager();
            var opties = new TransactionOptions();
            opties.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (var traLeverancierWijzigen = new TransactionScope(TransactionScopeOption.Required,opties))
            {
                using(var conPlanten = dbManager.GetConnection())
                {
                    using(var comWijzigen = conPlanten.CreateCommand())
                    {
                        comWijzigen.CommandType = CommandType.StoredProcedure;
                        comWijzigen.CommandText = "LeverancierWijzigen";

                        var parOudLevNr = comWijzigen.CreateParameter();
                        parOudLevNr.ParameterName = "OudLevNr";
                        parOudLevNr.Value = oudLevNr;
                        comWijzigen.Parameters.Add(parOudLevNr);

                        var parNieuwLevNr = comWijzigen.CreateParameter();
                        parNieuwLevNr.ParameterName = "NieuwLevNr";
                        parNieuwLevNr.Value = nieuwLevNr;
                        comWijzigen.Parameters.Add(parNieuwLevNr);

                        conPlanten.Open();
                        if (comWijzigen.ExecuteNonQuery() == 0)
                            throw new Exception("Ingegeven leverancier niet correct");
                    }
                
                    using(var comWissen = conPlanten.CreateCommand())
                    {
                        comWissen.CommandType = CommandType.StoredProcedure;
                        comWissen.CommandText = "LeverancierVerwijderen";

                        var parDelLevNr = comWissen.CreateParameter();
                        parDelLevNr.ParameterName = "LevNr";
                        parDelLevNr.Value = oudLevNr;
                        comWissen.Parameters.Add(parDelLevNr);

                        //conPlanten.Open();
                        if (comWissen.ExecuteNonQuery() == 0)
                            throw new Exception("Te verwijderen leverancier bestaat niet");
                        traLeverancierWijzigen.Complete();
                    }
                }
            }
        }
        public PlantenInfo PlantenGegevensOpvragen(int plantnr )
        {
            var dbManger = new TuinDBManager();
            using(var conTuin = dbManger.GetConnection())
            {
                using(var comOpzoeken = conTuin.CreateCommand())
                {
                    comOpzoeken.CommandType = CommandType.StoredProcedure;
                    comOpzoeken.CommandText = "GegevensOpzoeken";

                    var parPlantNr = comOpzoeken.CreateParameter();
                    parPlantNr.ParameterName = "@PlantNr";
                    parPlantNr.Value = plantnr;
                    comOpzoeken.Parameters.Add(parPlantNr);

                    var parNaam = comOpzoeken.CreateParameter();
                    parNaam.ParameterName = "@Naam";
                    parNaam.DbType = DbType.String;
                    parNaam.Size = 30;
                    parNaam.Direction = ParameterDirection.Output;
                    comOpzoeken.Parameters.Add(parNaam);

                    var parSoort = comOpzoeken.CreateParameter();
                    parSoort.ParameterName = "@Soort";
                    parSoort.DbType = DbType.String;
                    parSoort.Size = 10;
                    parSoort.Direction = ParameterDirection.Output;
                    comOpzoeken.Parameters.Add(parSoort);

                    var parLeverancierNaam = comOpzoeken.CreateParameter();
                    parLeverancierNaam.ParameterName = "@LeverancierNaam";
                    parLeverancierNaam.DbType = DbType.String;
                    parLeverancierNaam.Size = 30;
                    parLeverancierNaam.Direction = ParameterDirection.Output;
                    comOpzoeken.Parameters.Add(parLeverancierNaam);

                    var parKleur = comOpzoeken.CreateParameter();
                    parKleur.ParameterName = "@Kleur";
                    parKleur.DbType = DbType.String;
                    parKleur.Size = 10;
                    parKleur.Direction = ParameterDirection.Output;
                    comOpzoeken.Parameters.Add(parKleur);

                    var parKostprijs = comOpzoeken.CreateParameter();
                    parKostprijs.ParameterName = "@Kostprijs";
                    parKostprijs.DbType = DbType.Currency;
                    parKostprijs.Direction = ParameterDirection.Output;
                    comOpzoeken.Parameters.Add(parKostprijs);

                    conTuin.Open();
                    comOpzoeken.ExecuteNonQuery();

                    if (parKostprijs.Value.Equals(DBNull.Value))
                        throw new Exception("Plant nr " + parPlantNr.Value + " bestaat niet");
                    else
                        return new PlantenInfo((string)parNaam.Value, (string)parSoort.Value, (string)parLeverancierNaam.Value,
                            (string)parKleur.Value, (decimal)parKostprijs.Value);


                   
                }
            }

        }
        public List<Soort> GetSoorten()
        {
            List<Soort> soorten = new List<Soort>();
            var manager = new TuinDBManager();
            using(var conSoorten = manager.GetConnection())
            {
                using(var comSoortenZoeken = conSoorten.CreateCommand())
                {
                    comSoortenZoeken.CommandType = CommandType.Text;
                    comSoortenZoeken.CommandText = "select SoortNr,Soort from Soorten order by Soort";
                    conSoorten.Open();
                    using(var rdrSoorten = comSoortenZoeken.ExecuteReader())
                    {
                        Int32 soortNummerPos = rdrSoorten.GetOrdinal("SoortNr");
                        Int32 soortNaamPos = rdrSoorten.GetOrdinal("Soort");
                        while(rdrSoorten.Read())
                        {
                        soorten.Add(new Soort(rdrSoorten.GetInt32(soortNummerPos),rdrSoorten.GetString(soortNaamPos)));
                        }

                    }
                }
            }
            return soorten;
        }
        public List<Plant> GetPlanten(int soortNr)
        {
            List<Plant> planten = new List<Plant>();
            var manager = new TuinDBManager();
            using(var conPlanten = manager.GetConnection())
            {
                using(var comPlantenZoeken = conPlanten.CreateCommand())
                {
                    comPlantenZoeken.CommandType = CommandType.Text;
                    comPlantenZoeken.CommandText = "select * from Planten where SoortNr=@input order by Naam";

                    var parInput = comPlantenZoeken.CreateParameter();
                    parInput.ParameterName = "@input";
                    parInput.Value = soortNr;
                    comPlantenZoeken.Parameters.Add(parInput);
                    conPlanten.Open();
                    using(var rdrPlanten = comPlantenZoeken.ExecuteReader())
                    {
                        var plantNaamPos = rdrPlanten.GetOrdinal("Naam");
                        var plantNrPos = rdrPlanten.GetOrdinal("plantnr");
                        var levnrPos = rdrPlanten.GetOrdinal("levnr");
                        var prijsPos = rdrPlanten.GetOrdinal("verkoopprijs");
                        var kleurPos = rdrPlanten.GetOrdinal("kleur");
                        var soortPos = rdrPlanten.GetOrdinal("soortnr");

                        while(rdrPlanten.Read())
                        {
                            var eenPlant = new Plant(rdrPlanten.GetString(plantNaamPos),
                                rdrPlanten.GetInt32(plantNrPos),rdrPlanten.GetInt32(levnrPos),
                                rdrPlanten.GetDecimal(prijsPos),rdrPlanten.GetString(kleurPos));
                            planten.Add(eenPlant);
                        }
                    }
                }
            }
            return planten;
        }
        public void SchrijfWijzigingen(List<Plant> planten)
        {
            var manager = new TuinDBManager();
            using (var conTuin = manager.GetConnection())
            {
                using(var comUpdate = conTuin.CreateCommand())
                {
                    comUpdate.CommandType = CommandType.Text;
                    comUpdate.CommandText = "update planten set  Kleur=@plKleur, VerkoopPrijs=@plPrijs where PlantNr=@plPlantnr";

                    var parLevnr = comUpdate.CreateParameter();
                    parLevnr.ParameterName = "plLevnr";
                    comUpdate.Parameters.Add(parLevnr);

                    var parKleur = comUpdate.CreateParameter();
                    parKleur.ParameterName = "plKleur";
                    comUpdate.Parameters.Add(parKleur);

                    var parPrijs = comUpdate.CreateParameter();
                    parPrijs.ParameterName = "plPrijs";
                    comUpdate.Parameters.Add(parPrijs);

                    var parPlantNr = comUpdate.CreateParameter();
                    parPlantNr.ParameterName = "@plPlantnr";
                    comUpdate.Parameters.Add(parPlantNr);

                    conTuin.Open();
                    foreach (var eenPlant in planten)
                    {
                        parLevnr.Value = eenPlant.LevNr;
                        parPlantNr.Value = eenPlant.PlantNr;
                        parKleur.Value = eenPlant.Kleur;
                        parPrijs.Value = eenPlant.Prijs;
                        comUpdate.ExecuteNonQuery();

                    }
                }
            }
        }

        public ObservableCollection<Leverancier> GetLeveranciers()
        {
            ObservableCollection<Leverancier> leveranciers = new ObservableCollection<Leverancier>();
            var manager = new TuinDBManager();
            using (var conTuin = manager.GetConnection())
            {
                using (var comOntvangen = conTuin.CreateCommand())
                {
                    comOntvangen.CommandType = CommandType.Text;
                    comOntvangen.CommandText = "select * from Leveranciers";

                    conTuin.Open();
                    using (var rdrLeveranciers = comOntvangen.ExecuteReader())
                    {
                        Int32 levNrPos = rdrLeveranciers.GetOrdinal("LevNr");
                        Int32 levNaamPos = rdrLeveranciers.GetOrdinal("Naam");
                        Int32 levAdresPos = rdrLeveranciers.GetOrdinal("Adres");
                        Int32 levPostNrPos = rdrLeveranciers.GetOrdinal("PostNr");
                        Int32 levWoonplPos = rdrLeveranciers.GetOrdinal("Woonplaats");
                        Int32 levVersiePos = rdrLeveranciers.GetOrdinal("Versie");

                        while(rdrLeveranciers.Read())
                        {
                            leveranciers.Add(new Leverancier(rdrLeveranciers.GetInt32(levNrPos), rdrLeveranciers.GetString(levNaamPos),
                                rdrLeveranciers.GetString(levAdresPos), rdrLeveranciers.GetString(levPostNrPos), rdrLeveranciers.GetString(levWoonplPos),
                                rdrLeveranciers.GetValue(levVersiePos)));
                        }
                    }
                }
            }
            return leveranciers;
        }
        public List<String> GetPostCodes()
        {
            List<string> postnummers = new List<string>();
            var manager = new TuinDBManager();
            using(var conTuin = manager.GetConnection())
            {
                using(var comPostCodes = conTuin.CreateCommand())
                {
                    comPostCodes.CommandType = CommandType.StoredProcedure;
                    comPostCodes.CommandText = "PostCodes";
                    conTuin.Open();
                    using(var rdrPostCodes = comPostCodes.ExecuteReader())
                    {
                        Int32 postCodePos = rdrPostCodes.GetOrdinal("PostNr");
                        while(rdrPostCodes.Read())
                        {
                            postnummers.Add(rdrPostCodes.GetString(postCodePos).ToString());
                        }
                    }
                }
            }
            return postnummers;
        }
        public void SchrijfVerwijderingen(List<Leverancier> leveranciers)
        {
            var manager = new TuinDBManager();
            using(var conTuin = manager.GetConnection())
            {
                using(var comDelete = conTuin.CreateCommand())
                {
                    comDelete.CommandType = CommandType.Text;
                    comDelete.CommandText = "delete from leveranciers where LevNr=@levNr";
                    var parLevNr = comDelete.CreateParameter();
                    parLevNr.ParameterName = "@levNr";
                    comDelete.Parameters.Add(parLevNr);
                    conTuin.Open();
                    foreach (Leverancier eenLeverancier in leveranciers)
                    {
                        parLevNr.Value = eenLeverancier.LevNr;
                        comDelete.ExecuteNonQuery();
                    }
                }
            }
        }
        public void SchrijfToevoegingen(List<Leverancier> leveranciers)
        {
            var manager = new TuinDBManager();
            using(var conTuin = manager.GetConnection())
            {
                using(var comInsert = conTuin.CreateCommand())
                {
                    comInsert.CommandType = CommandType.Text;
                    comInsert.CommandText = "Insert into leveranciers(Naam,Adres,PostNr,Woonplaats) values (@naam,@adres,@postnr,@woonpl)";

                    var parNaam = comInsert.CreateParameter();
                    parNaam.ParameterName = "@naam";
                    comInsert.Parameters.Add(parNaam);

                    var parAdres = comInsert.CreateParameter();
                    parAdres.ParameterName = "@adres";
                    comInsert.Parameters.Add(parAdres);

                    var parPostNr = comInsert.CreateParameter();
                    parPostNr.ParameterName = "@postnr";
                    comInsert.Parameters.Add(parPostNr);

                    var parWoonpl = comInsert.CreateParameter();
                    parWoonpl.ParameterName = "@woonpl";
                    comInsert.Parameters.Add(parWoonpl);

                    conTuin.Open();

                    foreach (Leverancier eenLeverancier in leveranciers)
                    {
                        parNaam.Value = eenLeverancier.Naam;
                        parAdres.Value = eenLeverancier.Adres;
                        parPostNr.Value = eenLeverancier.PostNr;
                        parWoonpl.Value = eenLeverancier.Woonplaats;
                        comInsert.ExecuteNonQuery();
                    }
                }
            }
        }

        public void SchrijfWijzigingen(List<Leverancier> leveranciers)
        {
            var manager = new TuinDBManager();
            using(var conTuin = manager.GetConnection())
            {
                using(var comUpdate = conTuin.CreateCommand())
                {
                    comUpdate.CommandType = CommandType.Text;
                    comUpdate.CommandText = "update leveranciers set Naam=@naam,Adres=@adres, PostNr=@postnr,Woonplaats=@woonpl  where LevNr=@levnr and Versie=@versie";

                    var parNaam = comUpdate.CreateParameter();
                    parNaam.ParameterName = "@naam";
                    comUpdate.Parameters.Add(parNaam);

                    var parAdres = comUpdate.CreateParameter();
                    parAdres.ParameterName = "@adres";
                    comUpdate.Parameters.Add(parAdres);

                    var parPostNr = comUpdate.CreateParameter();
                    parPostNr.ParameterName = "@postnr";
                    comUpdate.Parameters.Add(parPostNr);

                    var parWoonpl = comUpdate.CreateParameter();
                    parWoonpl.ParameterName = "@woonpl";
                    comUpdate.Parameters.Add(parWoonpl);

                    var parLevNr = comUpdate.CreateParameter();
                    parLevNr.ParameterName = "@levnr";
                    comUpdate.Parameters.Add(parLevNr);

                    var parVersie = comUpdate.CreateParameter();
                    parVersie.ParameterName = "@versie";
                    comUpdate.Parameters.Add(parVersie);

                    conTuin.Open();

                    foreach (var eenleverancier in leveranciers)
                    {
                        parNaam.Value = eenleverancier.Naam;
                        parAdres.Value = eenleverancier.Adres;
                        parPostNr.Value = eenleverancier.PostNr;
                        parWoonpl.Value = eenleverancier.Woonplaats;
                        parLevNr.Value = eenleverancier.LevNr;
                        parVersie.Value = eenleverancier.Versie;
                        if (comUpdate.ExecuteNonQuery() == 0)
                            throw new Exception("Iemand was je voor");
                    }
                }
            }
        }
    }
}
