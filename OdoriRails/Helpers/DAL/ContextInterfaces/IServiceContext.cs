using System;
using System.Collections.Generic;
using System.Data;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.ContextInterfaces
{
    public interface IServiceContext
    {
        DataRow GetServiceById(int id);

        DataTable GetAllRepairsFromUser(User user);

        DataTable GetAllCleansFromUser(User user);

        DataTable GetAllRepairsWithoutUsers();

        DataTable GetAllCleansWithoutUsers();

        /// <summary>
        /// Edit de service in de database.
        /// </summary>
        /// <param name="service"></param>
        void EditService(Service service);

        /// <summary>
        /// </summary>
        /// Delete de service van de database.
        /// <param name="service"></param>
        void DeleteService(Service service);

        Cleaning AddCleaning(Cleaning cleaning);

        Repair AddRepair(Repair repair);

        DataTable GetAllRepairsFromTram(int tramId);

        DataTable GetAllCleaningsFromTram(int tramId);

        bool HadBigMaintenance(Tram tram);

        bool HadSmallMaintenance(Tram tram);

        /// <summary>
        /// Returnt een int[] met Repairs,Queries
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        int[] RepairsForDate(DateTime day);

        /// <summary>
        /// Returnt een int[] met bigclean, smallclean
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        int[] CleansForDate(DateTime day);

        DataTable GetUsersInServiceById(int i);
    }
}
