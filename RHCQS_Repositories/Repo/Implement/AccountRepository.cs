using Microsoft.EntityFrameworkCore;
using RHCQS_DataAccessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Repositories.Repo.Implement
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(RhcqsContext context) : base(context)
        {
        }
    }
}
