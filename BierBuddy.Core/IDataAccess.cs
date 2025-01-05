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
        public Visitor? GetAccount(long ID);
        /// <summary>
        /// geeft een lijst van accounts terug
        /// </summary>
        /// <param name="maxAmount"></param>
        /// <returns></returns>
        public List<Visitor> GetAccounts(int maxAmount); 
        /// <summary>
        /// geeft een lijst van accounts terug die de bezoeker nog niet heeft gezien
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="maxAmount"></param>
        /// <returns></returns>
        public List<Visitor> GetNotSeenAccounts(long ID, int maxAmount = 10);
        /// <summary>
        /// geeft een lijst van accounts terug die de bezoeker heeft geliked maar nog niet heeft gezien
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="maxAmount"></param>
        /// <returns></returns>
        public List<Visitor> GetLikedNotSeenAccounts(long ID, int maxAmount = 10);

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
        /// checkt als bezoekers gematched zijn
        /// </summary>
        /// <param name="ID1"></param>
        /// <param name="ID2"></param>
        /// <returns></returns>
        public bool CheckIfMatch(long ID1, long ID2);
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
        /// <summary>
        /// voeg een afspraak toe aan de database, deze is dan nog niet geaccepteerd
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="visitorID"></param>
        /// <param name="from"></param>
        /// <param name="till"></param>
        public void AddAppointment(long clientID, long visitorID, DateTime from, DateTime to);
        /// <summary>
        /// zet een afspraak op geaccepteerd
        /// </summary>
        public void ApproveAppointment(long appointmentID);
        /// <summary>
        /// verwijderd de afspraak uit de database
        /// </summary>
        public void DeclineAppointment(long appointmentID);
        /// <summary>
        /// haal alle afspraken op van de bezoeker
        /// </summary>
        public List<Appointment> GetAppointmentsFromUser(long clientID);
        /// <summary>
        /// haal alle afspraken op van de bezoeker met een andere bezoeker
        /// </summary>
        public List<Appointment> GetAppointmentsWithUser(long clientID, long visitorID);
        /// <summary>
        /// Haalt alle Buddies van een persoon op op basis van de huidige ClientID
        /// </summary>
        public List<Visitor> GetBuddies(long clientID);
        /// <summary>
        /// werkt een gebruiker bij met de gegeven gegevens, de lists zijn de ids van de activiteiten, drankjes en interesses
        /// </summary>
        public void UpdateAccount(Visitor visitor, List<long> activities, List<long> drinks, List<long> interests);
        /// <summary>
        /// Haalt naam op van een visitor op basis van ID, voor de appointments
        /// </summary>
        public string GetVisitorNameByID(long visitorID);

    }
}
