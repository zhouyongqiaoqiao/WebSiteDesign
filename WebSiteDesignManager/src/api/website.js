
import { $get,$post,$put,$delete ,getDataResult} from "../common/apiRequest"


//获取指定id的website图片详情
export let getWebSiteImageDetails = async(siteId)=>{
    var res =  await $get(`/WebSite/GetWebSiteImageDetails/${siteId}`);
    return getDataResult(res);
}

//根据名称或ID模糊查询WebSite分页
export let getWebSitePage = async(pageIndex,pageSize,searchKey)=>{
    var res =  await $get(`/WebSite/GetWebSitePage/${pageIndex}/${pageSize}/${searchKey}`);
    return getDataResult(res);
}

//根据主题查询WebSite分页
export let getWebSitePageByTheme = async(pageIndex,pageSize,themes,searchKey)=>{
    var res =  await $get(`/WebSite/GetWebSitePageByTheme/${pageIndex}/${pageSize}/${themes}/${searchKey}`);
    return getDataResult(res);
}

//添加website
export let addWebsite = async(name, theme, pageCount, price, description, sortNo, mainImageUrl)=>{
    description=description??"";
    mainImageUrl=mainImageUrl??"";
    var res =  await $post(`/WebSite/AddWebSite?name=${name}&theme=${theme}&pageCount=${pageCount}&price=${price}&description=${description}&sortNo=${sortNo}&mainImageUrl=${mainImageUrl}`);
    // var res =  await $post(`/WebSite/AddWebSite`,{'name':name, "theme": theme, "pageCount": pageCount, "price": price, "description": description, "sortNo": sortNo, "mainImageUrl": mainImageUrl});
    return getDataResult(res);
}

//更新website
export let updateWebSite = async(website)=>{
    var res =  await $post(`/WebSite/UpdateWebSite/`,website);
    return getDataResult(res);
}

//添加或更新website的图片详情
export let addOrUpdateWebSiteImage = async(siteID,imageDetails)=>{
    var res =  await $post(`/WebSite/UpdateWebSite/`,{'siteID':siteID ,'webSiteImageDetails':imageDetails});
    return getDataResult(res);
}

//启用指定website
export let enableUseWebSite = async(siteId)=>{
    var res =  await $put(`/WebSite/EnableUseWebSite/${siteId}`);
    return getDataResult(res);
}

//关闭指定website
export let stopUseWebSite = async(siteId)=>{
    var res =  await $put(`/WebSite/StopUseWebSite/${siteId}`);
    return getDataResult(res);
}

export default {
    getWebSiteImageDetails, getWebSitePage,getWebSitePageByTheme,addWebsite,
    updateWebSite,addOrUpdateWebSiteImage,enableUseWebSite,stopUseWebSite
}