
using BiologyManagement.Models.LoincFrance;
using BiologyManagement.Models.LoincFrance.EnumTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;

namespace BiologyManagement.Wpf.Db
{
    public class Management
    {
        string chaineconnection = "Server=XAVIER-DESKTOP\\BASESQL;Database=BiologieDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        string requeteChapitre;
        string requeteEchelle;
        string requeteGrandeur;
        string requeteMilieuBiologique;


        string requeteShortCodeChapitre = "Select Code_Short.Code_LOINC,Code_Short.Chapitre from Code_Short";
        string requeteShortCodeEchelle =  "Select Code_Short.Code_LOINC, Code_Short.Echelle from Code_Short";
        string requeteShortCodeGrandeur = "Select Code_Short.Code_LOINC, Code_Short.Grandeur from Code_Short";
        string requeteShortCodeMilieuBiologique = "Select Code_Short.Code_LOINC, Code_Short.Milieu_Biologique from Code_Short";
        string requeteShortCodeTechnique = "Select Code_Short.Code_LOINC, Code_Short.Technique from Code_Short";
        string requeteShortCodeTemps = "Select Code_Short.Code_LOINC, Code_Short.Temps from Code_Short";

        string requeteUpdateChapitre = "UPDATE Code_Short SET Code_Short.Chapitre = @Label WHERE Code_Short.Code_LOINC = @CodeLOINC";
        string requeteUpdateEchelle = "UPDATE Code_Short SET Code_Short.Echelle = @Label WHERE Code_Short.Code_LOINC = @CodeLOINC";
        string requeteUpdateGrandeur = "UPDATE Code_Short SET Code_Short.Grandeur = @Label WHERE Code_Short.Code_LOINC = @CodeLOINC";
        string requeteUpdateMilieuBiologique = "UPDATE Code_Short SET Code_Short.Milieu_Biologique = @Label WHERE Code_Short.Code_LOINC = @CodeLOINC";
        string requeteUpdateTechnique = "UPDATE Code_Short SET Code_Short.Technique = @Label WHERE Code_Short.Code_LOINC = @CodeLOINC";
        string requeteUpdateTemps = "UPDATE Code_Short SET Code_Short.Temps = @Label WHERE Code_Short.Code_LOINC = @CodeLOINC";

        List<ShortCode> shortCodes;
        List<ShortCode> newShortCodes;

        List<Chapitre> chapitres;
        List<Echelle> echelles;
        List<Grandeur> grandeurs;
        List<MilieuBiologique> milieuBiologiques;
        List<Technique> techniques;
        List<Temps> temps;
        List<VersionUtil> versionUtils;
        List<CodeUTIL> codeUTILs;
        List<VersionMAJ> versions;

        int nbChangement = 0;

        public int NbChangement { get => nbChangement; set => nbChangement = value; }

        /*****************REQUETE GENERIQUE********************/
        
            #region GENERIQUE
        private List<ShortCode> GetListeShortCode(String requete)
        {
            shortCodes = new List<ShortCode>();
            List<ShortCode> temporary = new List<ShortCode>();
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;


                using (SqlCommand commande = connection.CreateCommand())
                {

                    commande.CommandType = CommandType.Text;
                    commande.CommandText = requete;
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ShortCode c = new ShortCode();
                            c.CodeLoinc = reader[0].ToString();
                            c.Label = reader[1].ToString();
                            temporary.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            foreach (ShortCode s in temporary)
            {
                if (s.Label.Any(char.IsLetter))
                {
                    shortCodes.Add(s);
                }
            }


            return shortCodes;
        }

        //UPDATE

        public void Update(List<ShortCode> liste, string requete)
        {

            using (SqlConnection connection = new SqlConnection())
            {

                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {

                    commande.CommandType = CommandType.Text;
                    commande.CommandText = requete;
                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = "@Label";
                    p1.SqlDbType = SqlDbType.VarChar;
                    commande.Parameters.Add(p1);
                    SqlParameter p2 = new SqlParameter();
                    p2.ParameterName = "@CodeLOINC";
                    p2.SqlDbType = SqlDbType.VarChar;
                    commande.Parameters.Add(p2);

                    try
                    {
                        connection.Open();
                        foreach (ShortCode s in liste)
                        {
                            commande.Parameters[0].Value = s.Label;
                            commande.Parameters[1].Value = s.CodeLoinc;

                            commande.ExecuteNonQuery();

                        }
                    }
                    catch (Exception)
                    {

                    }

                }
                connection.Close();
            }

        }
        #endregion

        /*****************FIN REQUETE GENERIQUE********************/

        #region CHAPITRE
        //CHAPITRE
        public List<Chapitre> GetChapitres()
        {

            requeteChapitre = "Select TR_Chapitre.ChapitreId, TR_Chapitre.Label from TR_Chapitre";
            chapitres = new List<Chapitre>();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {
                    commande.CommandType = CommandType.Text;
                    commande.CommandText = requeteChapitre;
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chapitre c = new Chapitre();
                            c.ChapitreId = reader[0].ToString();
                            c.Label = reader[1].ToString();
                            chapitres.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            return chapitres;
        }

        public List<ShortCode> GetListeShortCodeChapître()
        {
           return  GetListeShortCode(requeteShortCodeChapitre);
        }

        public List<ShortCode> ChangeChapitre(List<ShortCode> liste)
        {
            newShortCodes = new List<ShortCode>();

            foreach (Chapitre c in chapitres)
            {
                foreach (ShortCode s in liste)
                {
                    if (c.Label == s.Label)
                    {
                        nbChangement += 1;
                        s.Label = c.ChapitreId;
                        newShortCodes.Add(s);
                    }
                }
            }

            return liste;
        }


        public void UpdateChapitre(List<ShortCode> liste)
        {

            Update(liste, requeteUpdateChapitre);
         

        }
        #endregion
       
        #region ECHELLE

        public List<Echelle> GetEchelles()
        {

            requeteEchelle = "SELECT TR_Echelle.EchelleId, TR_Echelle.Label from TR_Echelle";
            echelles = new List<Echelle>();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {
                    commande.CommandType = CommandType.Text;
                    commande.CommandText = requeteEchelle;
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Echelle c = new Echelle();
                            c.EchelleId = reader[0].ToString();
                            c.Label = reader[1].ToString();
                            echelles.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            return echelles;
        }

        public List<ShortCode> GetListeShortCodeEchelle()
        {

                return GetListeShortCode(requeteShortCodeEchelle);
        }

        public List<ShortCode> ChangeEchelle(List<ShortCode> liste)
        {
            newShortCodes = new List<ShortCode>();

            foreach (Echelle c in echelles)
            {
                foreach (ShortCode s in liste)
                {
                    if (c.Label == s.Label)
                    {
                        nbChangement += 1;
                        s.Label = c.EchelleId;
                        newShortCodes.Add(s);
                    }
                }
            }

            return liste;
        }

        public void UpdateEchelle(List<ShortCode> liste)
        {
            Update(liste, requeteUpdateEchelle);
        }
        #endregion

        #region GRANDEUR
        public List<Grandeur> GetGrandeur()
        {

            requeteGrandeur = "SELECT TR_Grandeur.GrandeurId, TR_Grandeur.Label from TR_Grandeur";
            grandeurs = new List<Grandeur>();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {
                    commande.CommandType = CommandType.Text;
                    commande.CommandText = requeteGrandeur;
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Grandeur c = new Grandeur();
                            c.GrandeurId = reader[0].ToString();
                            c.Label = reader[1].ToString();
                            grandeurs.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            return grandeurs;
        }

        public List<ShortCode> GetListeShortCodeGrandeur()
        {
            return GetListeShortCode(requeteShortCodeGrandeur);
        }

        public List<ShortCode> ChangeGrandeur(List<ShortCode> liste)
        {
            newShortCodes = new List<ShortCode>();

            foreach (Grandeur c in grandeurs)
            {
                foreach (ShortCode s in liste)
                {
                    if (c.Label == s.Label)
                    {
                        nbChangement += 1;
                        s.Label = c.GrandeurId;
                        newShortCodes.Add(s);
                    }
                }
            }

            return liste;
        }

        public void UpdateGrandeur(List<ShortCode> liste)
        {
            Update(liste, requeteUpdateGrandeur);
        }
        #endregion

        #region MILIEU BIOLOGIQUE
        public List<MilieuBiologique> GetMilieuBiologique()
        {
            milieuBiologiques = new List<MilieuBiologique>();
            requeteMilieuBiologique = "SELECT TR_MilieuBiologique.MilieuBiologiqueId, TR_MilieuBiologique.Label from TR_MilieuBiologique";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {
                    commande.CommandType = CommandType.Text;
                    commande.CommandText = requeteMilieuBiologique;
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MilieuBiologique c = new MilieuBiologique();
                            c.MilieuBiologiqueId = reader[0].ToString();
                            c.Label = reader[1].ToString();
                            milieuBiologiques.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            return milieuBiologiques;
        }

        public List<ShortCode> GetListeShortCodeMilieuBiologique()
        {
            return GetListeShortCode(requeteShortCodeMilieuBiologique);
        }


        public List<ShortCode> ChangeMilieuBiologique(List<ShortCode> liste)
        {
            newShortCodes = new List<ShortCode>();

            foreach (MilieuBiologique c in milieuBiologiques)
            {
                foreach (ShortCode s in liste)
                {
                    if (c.Label == s.Label)
                    {
                        nbChangement += 1;
                        s.Label = c.MilieuBiologiqueId;
                        newShortCodes.Add(s);
                    }
                }
            }

            return liste;
        }

        public void UpdateMilieuBiologique(List<ShortCode> liste)
        {
            Update(liste, requeteUpdateMilieuBiologique);
        }
        #endregion

        #region TECHNIQUE
        public List<Technique> GetTechniques()
        {
            techniques = new List<Technique>();
            requeteMilieuBiologique = "SELECT TR_Technique.TechniqueId, TR_Technique.Label from TR_Technique";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {
                    commande.CommandType = CommandType.Text;
                    commande.CommandText = requeteMilieuBiologique;
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Technique c = new Technique();
                            c.TechniqueId = reader[0].ToString();
                            c.Label = reader[1].ToString();
                            techniques.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            return techniques;
        }

        public List<ShortCode> GetListeShortCodeTechnique()
        {
            return GetListeShortCode(requeteShortCodeTechnique);
        }

        public List<ShortCode> ChangeTechnique(List<ShortCode> liste)
        {
            newShortCodes = new List<ShortCode>();

            foreach (Technique c in techniques)
            {
                foreach (ShortCode s in liste)
                {
                    if (c.Label == s.Label)
                    {
                        nbChangement += 1;
                        s.Label = c.TechniqueId;
                        newShortCodes.Add(s);
                    }
                }
            }

            return liste;
        }

        public void UpdateTechnique(List<ShortCode> liste)
        {
            Update(liste, requeteUpdateTechnique);
        }
        #endregion

        #region TEMPS
        public List<Temps> GetTemps()
        {
            temps = new List<Temps>();
            requeteMilieuBiologique = "SELECT TR_Temps.TempsId, TR_Temps.Label from TR_Temps";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {
                    commande.CommandType = CommandType.Text;
                    commande.CommandText = requeteMilieuBiologique;
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Temps c = new Temps();
                            c.TempsId = reader[0].ToString();
                            c.Label = reader[1].ToString();
                            temps.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            return temps;
        }

        public List<ShortCode> GetListeShortCodeTemps()
        {
            return GetListeShortCode(requeteShortCodeTemps);
        }

        public List<ShortCode> ChangeTemps(List<ShortCode> liste)
        {
            newShortCodes = new List<ShortCode>();

            foreach (Temps c in temps)
            {
                foreach (ShortCode s in liste)
                {
                    if (c.Label == s.Label)
                    {
                        nbChangement += 1;
                        s.Label = c.TempsId;
                        newShortCodes.Add(s);
                    }
                }
            }

            return liste;
        }

        public void UpdateTemps(List<ShortCode> liste)
        {
            Update(liste, requeteUpdateTemps);
        }
        #endregion

        public List<VersionMAJ> GetTR_Version()
        {

         
            versions = new List<VersionMAJ>();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {
                    commande.CommandType = CommandType.Text;
                    commande.CommandText = "Select TR_Version.VersionId, TR_Version.Label from TR_Version";
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VersionMAJ c = new VersionMAJ();
                            c.Id = (int)reader[0];
                            c.Label = reader[1].ToString();
                            versions.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            return versions;
        }




        public List<VersionUtil> GetVersionsRemplacement()
        {
            versionUtils = new List<VersionUtil>();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {
                    commande.CommandType = CommandType.Text;
                    commande.CommandText = "SELECT ID, Version FROM Remplacant WHERE VERSION IS NOT NULL";
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VersionUtil c = new VersionUtil();
                            c.VersionId = reader[0].ToString();
                            c.Label = reader[1].ToString();
                            versionUtils.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            return versionUtils;
        }

        public List<CodeUTIL> GetCodeRemplacement()
        {
            codeUTILs = new List<CodeUTIL>();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {
                    commande.CommandType = CommandType.Text;
                    commande.CommandText = "SELECT ID, Code FROM Remplacant WHERE CODE IS NOT NULL";
                    connection.Open();
                    using (SqlDataReader reader = commande.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CodeUTIL c = new CodeUTIL();
                            c.CodeUTILId = reader[0].ToString();
                            c.Label = reader[1].ToString();
                            codeUTILs.Add(c);

                        }
                    }
                }
                connection.Close();
            }

            return codeUTILs;
        }

        public void TrTDataVersion (List<VersionUtil> versions)
        {
            foreach(VersionUtil v in versions)
            {
                string[] words = v.Label.Trim().Split(',');

                for (int i = 0; i < words.Length; ++i)
                {
                    v.SetLabel(words[i].Trim());
                }
            }
        }

        public void TrTDataCode(List<CodeUTIL> codes)
        {
            foreach (CodeUTIL v in codes)
            {
                string[] words = v.Label.Trim().Split(',');

                for (int i = 0; i < words.Length; ++i)
                {
                    v.SetLabel(words[i].Trim());
                }
            }
        }

        public List<VersionMAJ> SetListeActeVersion(List<VersionUtil> versionUtils, List<VersionMAJ> versions)
        {
            List<VersionMAJ> acteVersions = new List<VersionMAJ>();

            foreach (VersionMAJ v in versions)
            {
                
                foreach(VersionUtil u in versionUtils)
                {

                    for(int i =0; i< u.Liste.Count; ++i)
                    if(v.Label == u.Liste[i])
                    {
                        VersionMAJ newActeVersion = new VersionMAJ();
                        newActeVersion.Id = v.Id;
                        newActeVersion.Label = u.VersionId;
                        acteVersions.Add(newActeVersion);
                    }
                }
            }

            return acteVersions;
        }

        public void SetCode(List<CodeUTIL> listes)
        {
            using (SqlConnection connection = new SqlConnection())
            {

                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {

                    commande.CommandType = CommandType.Text;
                    commande.CommandText = "INSERT INTO TD_Remplacement(ActeBiologiqueId, ActeID) values (@ActeBiologiqueId, @ActeID)";
                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = "@ActeBiologiqueId";
                    p1.SqlDbType = SqlDbType.VarChar;
                    commande.Parameters.Add(p1);
                    SqlParameter p2 = new SqlParameter();
                    p2.ParameterName = "@ActeID";
                    p2.SqlDbType = SqlDbType.VarChar;
                    commande.Parameters.Add(p2);

                    try
                    {
                        connection.Open();
                        foreach (CodeUTIL s in listes)
                        {
                            for(int i =0; i < s.Liste.Count; ++i)
                            {
                                commande.Parameters[0].Value = s.CodeUTILId;

                                commande.Parameters[1].Value = s.Liste[i];

                                commande.ExecuteNonQuery();
                            }



                        }
                    }
                    catch (Exception)
                    {

                    }

                }
                connection.Close();
            }
        }

        public void SetVersion(List<VersionMAJ> listes)
        {
            using (SqlConnection connection = new SqlConnection())
            {

                connection.ConnectionString = chaineconnection;

                using (SqlCommand commande = connection.CreateCommand())
                {

                    commande.CommandType = CommandType.Text;
                    commande.CommandText = "INSERT INTO TJ_ActebiologiqueVersion(ActeBiologiqueId, VersionID) values (@ActeBiologiqueId, @VersionID)";
                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = "@ActeBiologiqueId";
                    p1.SqlDbType = SqlDbType.VarChar;
                    commande.Parameters.Add(p1);
                    SqlParameter p2 = new SqlParameter();
                    p2.ParameterName = "@VersionID";
                    p2.SqlDbType = SqlDbType.Int;
                    commande.Parameters.Add(p2);

                    try
                    {
                        connection.Open();
                        foreach (VersionMAJ s in listes)
                        {
                        
                         
                                commande.Parameters[0].Value = s.Label;

                                commande.Parameters[1].Value = s.Id;

                                commande.ExecuteNonQuery();
                  



                        }
                    }
                    catch (Exception)
                    {

                    }

                }
                connection.Close();
            }
        }




    }



}
