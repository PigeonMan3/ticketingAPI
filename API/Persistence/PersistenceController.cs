using API.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistence
{
    internal class PersistenceController
    {
        ////////////////////////////////////////////////////////////////////////////////USERS/////////////////////////////////////////////////////////////////////////////////////////
        
        public List<User> GetUsers(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            return MU.GetUsers(UID);
        }
        public List<User> GetAdminUsers(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            return MU.GetAdminUsers(UID);
        }
        public List<User> GetUserData(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            return MU.GetUserData(UID);
        }
        public void CreateUser(User u)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.CreateUser(u);
        }
        public void DeleteUser(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.DeleteUser(UID);
        }

        public string GetPassword(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            return MU.GetPassword(UID);
        }
        public void CreatePassword(string pass, int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.CreatePassword(pass, UID);
        }
        public void DeletePassword(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.DeletePassword(UID);
        }

        public int GetUID(string email)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            return MU.GetUID(email);
        }
        public bool GetVerification(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            return MU.CheckVerification(UID);
        }
        public string GetUserPerms(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            return MU.GetUserPermission(UID);
        }
        public bool CheckUsernameAvailability(string Username)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            return MU.CheckUsernameAvailability(Username);
        }
        public void VerifyUser(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.VerifyUser(UID);
        }
        public void ChangeUserType(int UID, string goal)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.ChangeUserType(UID, goal);
        }

        public void PermDeleteU_DeleteAssignments(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.PermDeleteU_DeleteAssignments(UID);
        }

        public void PermDeleteU_DeleteComments(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.PermDeleteU_DeleteComments(UID);
        }

        public void PermDeleteU_DeletePasswords(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.PermDeleteU_DeletePasswords(UID);
        }

        public void PermDeleteU_DeleteTickets(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.PermDeleteU_DeleteTickets(UID);
        }

        public void PermDeleteU_DeleteSessions(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.PermDeleteU_DeleteSessions(UID);
        }

        public void PermanentlyDeleteUsers(int UID)
        {
            Persistence.MapperUser MU = new Persistence.MapperUser();
            MU.PermanentlyDeleteUsers(UID);
        }




        ////////////////////////////////////////////////////////////////////////////////TICKETS/////////////////////////////////////////////////////////////////////////////////////////

        public void CreateTicket(Ticket ticket)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.AddTicket(ticket);
        }
        public List<Ticket> GetAllTickets(int UID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            return MT.GetAllTickets(UID);
        }
        public List<Ticket> GetOwnTickets(int UID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            return MT.GetOwnTickets(UID);
        }

        public List<Ticket> GetArchivedTickets(int UID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            return MT.GetArchivedTickets(UID);
        }
        public List<Ticket> GetTicketByID(int ID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            return MT.GetTicketById(ID);
        }
        public void DeleteTicket(int TicketID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.DeleteTicket(TicketID);
        }
        public void ArchiveTicket(int TicketID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.ArchiveTicket(TicketID);
        }

        public void UnarchiveTicket(int TicketID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.UnarchiveTicket(TicketID);
        }
        public void UpdateTicket(Ticket t, int TicketID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.UpdateTicket(t, TicketID);
        }

        public void CloseTicket(int TicketID, int UID, string ResolvedNotes)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.CloseTicket(TicketID, UID, ResolvedNotes);
        }
        public void AddResolver(int TicketID, int UID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.AddResolver(TicketID, UID);
        }
        public void AssignUser(int UID, int TID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.AssignUser(UID, TID);
        }
        public void UnassignUser(int UID, int TID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.UnassignUser(UID, TID);
        }
        public List<User> GetAssignedUsers(int TID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            return MT.GetAssignedUsers(TID);
        }

        public void PermanentlyDeleteTickets(int UID)
        {
            Persistence.MapperTickets MT = new Persistence.MapperTickets();
            MT.PermanentlyDelete(UID);
        }


        ////////////////////////////////////////////////////////////////////////////////TICKET_ATTRIBUTES/////////////////////////////////////////////////////////////////////////////////////////
        public List<Priority> GetPriors()
        {
            Persistence.MapperTicketAttributes MTA = new Persistence.MapperTicketAttributes();
            return MTA.GetPriors();
        }
        public List<Categories> GetCats(int UID)
        {
            Persistence.MapperTicketAttributes MTA = new Persistence.MapperTicketAttributes();
            return MTA.GetCats(UID);
        }
        public List<Status> GetStatuses()
        {
            Persistence.MapperTicketAttributes MTA = new Persistence.MapperTicketAttributes();
            return MTA.GetStatuses();
        }

        public void ChangeStatus(string Status, int TID)
        {
            Persistence.MapperTicketAttributes MTA = new Persistence.MapperTicketAttributes();
            MTA.ChangeStatus(Status, TID);
        }
        public void AddCategory(string Cat)
        {
            Persistence.MapperTicketAttributes MTA = new Persistence.MapperTicketAttributes();
            MTA.AddCategory(Cat);
        }
        public void PermDeleteT_DeleteComments(int UID)
        {
            Persistence.MapperTicketAttributes MTA = new Persistence.MapperTicketAttributes();
            MTA.PermanentlyDelete_DeleteComments(UID);
        }
        public void PermDeleteT_DeleteAssignments(int UID)
        {
            Persistence.MapperTicketAttributes MTA = new Persistence.MapperTicketAttributes();
            MTA.PermanentlyDelete_DeleteAssignments(UID);
        }

        public void DeleteAssignmentsOfTicket(int TicketID)
        {
            Persistence.MapperTicketAttributes MTA = new Persistence.MapperTicketAttributes();
            MTA.DeleteAssignmentsOfTicket(TicketID);
        }
        public void DeleteCommentsOfTicket(int TicketID)
        {
            Persistence.MapperTicketAttributes MTA = new Persistence.MapperTicketAttributes();
            MTA.DeleteCommentsOfTicket(TicketID);
        }


        ////////////////////////////////////////////////////////////////////////////////SESSIONS/////////////////////////////////////////////////////////////////////////////////////////
        public string GetSession(int UID)
        {
            Persistence.MapperSessions MS = new Persistence.MapperSessions();
            return MS.GetSession(UID);
        }

        public void AddSession(int UID, string SESID)
        {
            Persistence.MapperSessions MS = new Persistence.MapperSessions();
            MS.AddSession(SESID, UID);
        }

        public void DeleteSession(int UID)
        {
            Persistence.MapperSessions MS = new Persistence.MapperSessions();
            MS.DeleteSession(UID);
        }

        public void UpdateSession(int UID, string Key)
        {
            Persistence.MapperSessions MS = new Persistence.MapperSessions();
            MS.UpdateSession(UID, Key);
        }

        ////////////////////////////////////////////////////////////////////////////////COMPANY/////////////////////////////////////////////////////////////////////////////////////////
        public void AddCompany(Company c)
        {
            Persistence.MapperCompany MC = new Persistence.MapperCompany();
            MC.addCompany(c);
        }
        public void AddCategoryToCompany(int UID, string Cat)
        {
            Persistence.MapperCompany MC = new Persistence.MapperCompany();
            MC.AddCategoryToCompany(Cat, UID);
        }
        public void DeleteCategory(string Cat, int UID)
        {
            Persistence.MapperCompany MC = new Persistence.MapperCompany();
            MC.DeleteCategory(Cat, UID);
        }
        public int GetCompanyByDomainAndGetCID(string domain)
        {
            Persistence.MapperCompany MC = new Persistence.MapperCompany();
            return MC.GetCompanyByDomainAndGetCID(domain);
        }


        ////////////////////////////////////////////////////////////////////////////////COMMENTS/////////////////////////////////////////////////////////////////////////////////////////

        public List<Comment> GetComments(int TicketID, bool IsAdminOnly)
        {
            Persistence.MapperComments MC = new Persistence.MapperComments();
            return MC.GetComments(TicketID, IsAdminOnly);
        }
        public void AddComment(Comment c)
        {
            Persistence.MapperComments MC = new Persistence.MapperComments();
            MC.AddComment(c);
        }

        ////////////////////////////////////////////////////////////////////////////////STATS/////////////////////////////////////////////////////////////////////////////////////////

        public int GetUnresolvedTicketsStats(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetUnresolvedTicketsStats(UID);
        }
        public int GetNewTicketsStats(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetNewTicketsStats(UID);
        }
        public int GetCriticalTicketsStats(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetCriticalTicketsStats(UID);
        }
        public string GetBestResolver(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetBestResolver(UID);
        }
        public int GetUpcomingDeadlines(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetUpcomingDeadlines(UID);
        }
        public Tuple<string, int> GetMostCommonTicketCategory(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetMostCommonTicketCategory(UID);
        }

        public Dictionary<string, int> GetTicketsByCategory(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetTicketsByCategory(UID);
        }
        public Dictionary<string, int> GetTicketsByStatus(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetTicketsByStatus(UID);
        }

        public Dictionary<string, int> GetOwnTicketsByStatus(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetOwnTicketsByStatus(UID);
        }

        public Dictionary<string, int> GetOwnTicketsByCategory(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetOwnTicketsByCategory(UID);
        }

        public int GetOwnUnresolvedTicketsStats(int UID)
        {
            Persistence.MapperStatistics MS = new Persistence.MapperStatistics();
            return MS.GetOwnUnresolvedTicketsStats(UID);
        }
    }
}
