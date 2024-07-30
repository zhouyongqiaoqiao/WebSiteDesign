using ProjectWebApiCore.Result;
using SqlSugar;
using System.Linq.Expressions;

namespace ProjectWebApiCore.Interface
{
    public interface IBaseService
    {

        #region Query
        /// <summary>
        /// 主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Find<T>(object id) where T : class;

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetAll<T>() where T : class;

        /// <summary>
        /// 主键查询-异步版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindAsync<T>(object id) where T : class;

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        ISugarQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        PagingData<T> QueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, object>> funcOrderby, bool isAsc = true) where T : class;
        #endregion

        #region Add

        /// <summary>
        /// 新增数据-同步版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        T Insert<T>(T t) where T : class, new();

        /// <summary>
        /// 新增数据-异步版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<T> InsertAsync<T>(T t) where T : class, new();

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        Task<bool> InsertList<T>(List<T> tList) where T : class, new();
        #endregion

        #region Update
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync<T>(T t) where T : class, new();

        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <param name="tList"></param>
        void Update<T>(List<T> tList) where T : class, new();
        #endregion

        #region Delete
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pId"></param>
        /// <returns></returns>
        bool Delete<T>(object pId) where T : class, new();

        /// <su+mary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        void Delete<T>(T t) where T : class, new();

        /// <summary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <param name="tList"></param>
        void Delete<T>(List<T> tList) where T : class;
        #endregion

        #region Other

        /// <summary>
        /// 执行sql 返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        ISugarQueryable<T> ExcuteQuery<T>(string sql) where T : class, new();

        #endregion





        #region MyRegion

        //public void Add();
        //public void Update();
        //public void Query();
        //public void Delete();

        #endregion
    }
}
