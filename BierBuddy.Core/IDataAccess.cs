using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    internal interface IDataAccess
    {
        /// <summary>
        /// haalt een account op op basis van een account ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Bezoeker GetAccount(int ID);
        /// <summary>
        /// voegt een account toe aan de database
        /// </summary>
        /// <param name="bezoeker"></param>
        public void AddAccount(Bezoeker bezoeker);
        /// <summary>
        /// zet een like op een bezoeker
        /// </summary>
        /// <param name="likerID">de id van degene die iemand anders liked</param>
        /// <param name="gelikedeID">de id van degene die geliked is</param>
        public void SetLike(int likerID, int gelikedeID);
        /// <summary>
        /// zet een dislike op een bezoeker
        /// </summary>
        /// <param name="dislikerID">de id van degene die iemand anders disliked</param>
        /// <param name="gedislikedeID">de id van degene die gedisliked is</param>
        public void SetDislike(int dislikerID, int gedislikedeID);
        /// <summary>
        /// haal alle IDs op van mensen die de bezoeker hebben gedisliked
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<int> GetGegevenLikes(int ID);
        /// <summary>
        /// haal alle IDs op van mensen die de bezoeker hebben geliked
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<int> GetOntvangenLikes(int ID);
        /// <summary>
        /// haal alle IDs op van de bezoekers die een match hebben met de bezoeker
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<int> GetMatches(int ID);
    }
}
