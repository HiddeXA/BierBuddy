using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public interface IDataAccess
    {
        /// <summary>
        /// haalt een account op op basis van een account ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Visitor GetAccount(ulong ID);
        /// <summary>
        /// gives a list of random accounts
        /// </summary>
        /// <param name="maxAmount"></param>
        /// <returns></returns>
        public List<Visitor> GetAccounts(int maxAmount); 
        /// <summary>
        /// gives a list of random accounts that are not liked or disliked by the visitor
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="maxAmount"></param>
        /// <returns></returns>
        public List<Visitor> GetNotSeenAccounts(ulong ID, int maxAmount = 10);
        /// <summary>
        /// gives a list of random accounts that are not liked or disliked by the visitor but have liked the visitor
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="maxAmount"></param>
        /// <returns></returns>
        public List<Visitor> GetLikedNotSeenAccounts(ulong ID, int maxAmount = 10);
        /// <summary>
        /// voegt een account toe aan de database
        /// </summary>
        /// <param name="visitor"></param>
        public void AddAccount(Visitor visitor);
        /// <summary>
        /// zet een like op een bezoeker
        /// </summary>
        /// <param name="likerID">de id van degene die iemand anders liked</param>
        /// <param name="likedID">de id van degene die geliked is</param>
        public void SetLike(ulong likerID, ulong likedID);
        /// <summary>
        /// zet een dislike op een bezoeker
        /// </summary>
        /// <param name="dislikerID">de id van degene die iemand anders disliked</param>
        /// <param name="dislikedID">de id van degene die gedisliked is</param>
        public void SetDislike(ulong dislikerID, ulong dislikedID);
        /// <summary>
        /// haal alle IDs op van mensen die de bezoeker hebben gedisliked
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<int> GetGivenLikes(ulong ID);
        /// <summary>
        /// haal alle IDs op van mensen die de bezoeker hebben geliked
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<int> GetReceivedLikes(ulong ID);
        /// <summary>
        /// haal alle IDs op van de bezoekers die een match hebben met de bezoeker
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<int> GetMatches(ulong ID); 
        /// <summary>
        /// checkt als bezoekers gematched zijn
        /// </summary>
        /// <param name="ID1"></param>
        /// <param name="ID2"></param>
        /// <returns></returns>
        public bool CheckIfMatch(ulong ID1, ulong ID2);
    }
}
