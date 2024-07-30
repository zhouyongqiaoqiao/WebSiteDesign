using AutoMapper.Internal.Mappers;
using ProjectWebApiCore.Interface;
using ProjectWebApiCore.Result;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApiCore.Service
{
    public abstract class BaseService : IBaseService
    {
        protected ISqlSugarClient SqlClient { get; set; }
        public BaseService(ISqlSugarClient sqlSugarClient)
        {
            this.SqlClient = sqlSugarClient;
        }

        #region Insert
        /// <summary>
        /// 新增数据-同步版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Insert<T>(T t) where T : class, new()
        {
            return this.SqlClient.Insertable(t).ExecuteReturnEntity();
        }

        /// <summary>
        /// 新增数据-异步版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public Task<T> InsertAsync<T>(T t) where T : class, new()
        {
           return this.SqlClient.Insertable(t).ExecuteReturnEntityAsync();
        }

        /// <summary>
        /// 批量新增-事务执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        public Task<bool> InsertList<T>(List<T> tList) where T : class, new()
        {
            return this.SqlClient.Insertable(tList).ExecuteCommandIdentityIntoEntityAsync();
        }
        #endregion

        #region Delete
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pId">主键</param>
        /// <returns></returns>
        public bool Delete<T>(object pId) where T : class, new()
        {
            T t = this.SqlClient.Queryable<T>().InSingle(pId);
            return this.SqlClient.Deleteable(t).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Delete<T>(T t) where T : class, new()
        {
            this.SqlClient.Deleteable(t).ExecuteCommand();
        }

        public void Delete<T>(List<T> tList) where T : class
        {
            this.SqlClient.Deleteable(tList).ExecuteCommand();
        }

        public bool Delete<T>(Expression<Func<T, bool>> funcWhere) where T : class, new()
        {
            return this.SqlClient.Deleteable<T>(funcWhere).ExecuteCommand() > 0;
        }
        #endregion

        #region Update

        public void Update<T>(List<T> tList) where T : class, new()
        {
            this.SqlClient.Updateable(tList).ExecuteCommand();

        }

        /// <summary>
        /// 异步更新， 其中未实现查询该T，再进行更新，实际
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UpdateAsync<T>(T t) where T : class, new()
        {
            if (t == null) { throw new Exception("t is null"); }
            return await this.SqlClient.Updateable(t).ExecuteCommandHasChangeAsync();
        }
        #endregion

        #region Query


        /// <summary>
        /// 直接执行SQL脚本，返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public ISugarQueryable<T> ExcuteQuery<T>(string sql) where T : class, new()
        {
              return this.SqlClient.SqlQueryable<T>(sql);
        }

        /// <summary>
        /// 主键查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(object id) where T : class
        {
            return this.SqlClient.Queryable<T>().InSingle(id);
        }

        /// <summary>
        /// 主键查询-异步版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> FindAsync<T>(object id) where T : class
        {
            return this.SqlClient.Queryable<T>().InSingleAsync(id);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<List<T>> GetAll<T>() where T : class
        {
            return this.SqlClient.Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public ISugarQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return this.SqlClient.Queryable<T>().Where(funcWhere);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere">查询条件</param>
        /// <param name="pageSize">每页最大数量</param>
        /// <param name="pageIndex">第几页（索引从1 开始）</param>
        /// <param name="funcOrderby">排序规则</param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public PagingData<T> QueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, object>> funcOrderby, bool isAsc = true) where T : class
        {
            var queryable = this.SqlClient.Queryable<T>();
            if (funcWhere != null)
            {
                queryable = queryable.Where(funcWhere);
            }
             queryable = queryable.OrderByIF(true, funcOrderby, isAsc ? OrderByType.Asc : OrderByType.Desc);
            int totalCount = 0;
            List<T> list = queryable.ToPageList(pageIndex , pageSize, ref totalCount);
            PagingData<T> result = new PagingData<T>()
            {
                DataList = list,
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = totalCount
            };
            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressionable">查询条件集合</param>
        /// <param name="pageSize">每页最大数量</param>
        /// <param name="pageIndex">第几页（索引从1 开始）</param>
        /// <param name="funcOrderby">排序规则</param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public PagingData<T> QueryPage<T>(Expressionable<T> expressionable, int pageSize, int pageIndex, Expression<Func<T, object>> funcOrderby, bool isAsc = true) where T : class, new()
        {
            Expression<Func<T, bool>> funcWhere = expressionable == null? null : expressionable.ToExpression();
            return this.QueryPage<T>(funcWhere, pageSize, pageIndex, funcOrderby,isAsc);
        }
        #endregion

        public void Dispose()
        {
            if (this.SqlClient != null)
            {
                this.SqlClient.Dispose();
            }
        }

    }
}
