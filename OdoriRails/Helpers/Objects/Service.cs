using System;
using System.Collections.Generic;

namespace OdoriRails.Helpers.Objects
{
    public abstract class Service
    {
        protected Service(int? id, List<User> assignedUsers, DateTime startDate, DateTime? endDate, int? tramId)
        {
            Id = id ?? -1;
            AssignedUsers = assignedUsers;
            StartDate = startDate;
            EndDate = endDate;
            TramId = tramId ?? -1;
        }

        protected Service(List<User> assignedUsers, DateTime startDate, DateTime? endDate, int tramId)
        {
            AssignedUsers = assignedUsers;
            StartDate = startDate;
            EndDate = endDate;
            TramId = tramId;
        }

        public int Id { get; private set; }
        public List<User> AssignedUsers { get; }
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; set; }
        public int TramId { get; }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}