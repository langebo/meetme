using System;

namespace MeetMe.Domain.Models
{
    public class Vote
    {
        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
    }
}