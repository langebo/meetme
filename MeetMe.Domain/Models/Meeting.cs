using System;
using System.Collections.Generic;

namespace MeetMe.Domain.Models
{
    public class Meeting
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public User Creator { get; set; }
        public List<Proposal> Proposals { get; set; }
        public List<Invitation> Invitations { get; set; }

        public Meeting()
        {
            Proposals = new List<Proposal>();
            Invitations = new List<Invitation>();
        }
    }
}