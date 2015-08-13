﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


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
                    parPostNr.ParameterName = "@Postcode";
                    parPostNr.Value = eenLeverancier.PostNr;
                    comToevoegen.Parameters.Add(parPostNr);
                    var parWoonplaats = comToevoegen.CreateParameter();
                    parWoonplaats.ParameterName = "@Woonplaats";
                    parWoonplaats.Value = eenLeverancier.Woonplaats;
                    comToevoegen.Parameters.Add(parWoonplaats);

                    conTuin.Open();
                    comToevoegen.ExecuteNonQuery();
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

    }
}