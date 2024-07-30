//定义管理员API，
//导入请求函数
import { $get,$post,$put,$delete ,getDataResult} from "../common/apiRequest"

//获取系统日志分页
export let getSystemLogPage = async(pageIndex,pageSize)=>{
    var res =  await $get(`/System/${pageIndex}/${pageSize}`);
    return getDataResult(res);
}

//获取系统日志分页 （包含日期区间）
export let getSystemLogPage2 = async(pageIndex,pageSize,startDate,endDate)=>{
    var res =  await $get(`/System/${pageIndex}/${pageSize}/${startDate}/${endDate}`);
    return getDataResult(res);
}

export default {
    getSystemLogPage, getSystemLogPage2
}
