using ProjectWebApiCore.Interface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApiCore.Service
{
    public class SystemlogService : BaseService, ISystemlogService
    {
        public SystemlogService(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
        }

        public void InsertlogAndUpdateLog()
        {
            
        }
    }
}
