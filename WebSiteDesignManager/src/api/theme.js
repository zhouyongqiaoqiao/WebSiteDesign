
import { $get,$post,$put,$delete ,getDataResult} from "../common/apiRequest"

//按搜索关键字获取主题
export let getThemes = async(searchKey)=>{
    var res =  await $get(`/Theme/GetThemes/${searchKey}`);
    return getDataResult(res);
}

//获取所有主题
export let getAllThemes = async()=>{
    var res =  await $get(`/Theme/GetAllThemes/`);
    return getDataResult(res);
}

//添加主题
export let addTheme = async(theme)=>{
    var res =  await $post(`/Theme/AddTheme`,theme);
    return getDataResult(res);
}

//更新主题
export let updateTheme = async(theme)=>{
    var res =  await $post(`/Theme/UpdateTheme/`,theme);
    return getDataResult(res);
}

//删除主题
export let deleteTheme = async(name)=>{
    var res =  await $delete(`/Theme/DeleteTheme/${name}`);
    return getDataResult(res);
}

export default {
    getThemes, getAllThemes,addTheme,updateTheme,deleteTheme
}