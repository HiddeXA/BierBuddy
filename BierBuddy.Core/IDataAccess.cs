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
        public Visitor? GetAccount(long ID);
        /// <summary>
        /// voegt een account toe aan de database
        /// </summary>
        /// <param name="activities">de activiteiten van de bezoeker, minimaal 1, maximaal 4</param>
        /// <param name="drinks">de drankjes van de bezoeker, minimaal 1, maximaal 4</param>
        /// <param name="interests">de interesses van de bezoeker, minimaal 1, maximaal 4</param>
        /// <param name="photos">de foto's van de bezoeker, minimaal 1, maximaal 4</param>
        /// <returns>het aangemaakte account</returns>
        /// <exception cref="ArgumentException">als er minder dan 1 of meer dan 4 activiteiten, drankjes, interesses of foto's worden meegegeven</exception>"
        public Visitor? AddAccount(string name, string bio, int age, List<long> activities, List<long> drinks, List<long> interests, List<string> photos);
        /// <summary>
        /// zet een like op een bezoeker
        /// </summary>
        /// <param name="likerID">de id van degene die iemand anders liked</param>
        /// <param name="likedID">de id van degene die geliked is</param>
        public void SetLike(long likerID, long likedID);
        /// <summary>
        /// zet een dislike op een bezoeker
        /// </summary>
        /// <param name="dislikerID">de id van degene die iemand anders disliked</param>
        /// <param name="dislikedID">de id van degene die gedisliked is</param>
        public void SetDislike(long dislikerID, long dislikedID);
        /// <summary>
        /// haal alle IDs op van mensen die de bezoeker hebben gedisliked
        /// </summary>
        public List<long> GetGivenLikes(long ID);
        /// <summary>
        /// haal alle IDs op van mensen die de bezoeker hebben geliked
        /// </summary>
        public List<long> GetReceivedLikes(long ID);
        /// <summary>
        /// haal alle IDs op van de bezoekers die een match hebben met de bezoeker
        /// </summary>
        public List<long> GetMatches(long ID);
        /// <summary>
        /// haal alle mogelijke activiteiten op uit de database
        /// </summary>
        /// <returns>een Dictionary met alle mogelijke activiteiten met hun bijbehorende index</returns>
        public Dictionary<long, String> GetPossibleActivities();
        /// <summary>
        /// haal alle mogelijke drankjes op uit de database
        /// </summary>
        /// <returns>een Dictionary met alle mogelijke drankjes met hun bijbehorende index</returns>
        public Dictionary<long, String> GetPossibleDrinks();
        /// <summary>
        /// haal alle mogelijke interesses op uit de database
        /// </summary>
        /// <returns>een Dictionary met alle mogelijke interesses met hun bijbehorende index</returns>
        public Dictionary<long, String> GetPossibleInterests();
    }
}
