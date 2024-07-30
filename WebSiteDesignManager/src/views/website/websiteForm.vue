<!-- WebSiteForm.vue -->
<template>
  <el-form  :model="website" :rules="rules" ref="formRef" label-position="left" label-width="100px">
    <el-form-item v-if="!props.isAdd" label="网站编号" prop="id">
      <el-input  v-model="website.id" :disabled="!props.isAdd"></el-input>
    </el-form-item>
    <el-form-item label="网站名称" prop="name">
      <el-input v-model="website.name"></el-input>
    </el-form-item>
    <el-form-item label="主题">
      <el-checkbox-group v-model="checkThemeList" @change ="checkThemeListChange">
        <el-checkbox v-for="theme in themeList" :label="theme.name" :value="theme.name" />
      </el-checkbox-group>
    </el-form-item>
    <el-form-item label="页数" prop="pageCount">
      <el-input-number min="1" v-model="website.pageCount"></el-input-number>
    </el-form-item>
    <el-form-item label="价格" prop="price">
      <el-input-number min="0" :precision="2" v-model="website.price"></el-input-number>
    </el-form-item>

    <el-form-item label="详情" prop="description">
      <el-input v-model="website.description"></el-input>
    </el-form-item>
    <el-form-item label="排序" prop="sortNo">
      <el-input v-model="website.sortNo"></el-input>
    </el-form-item>
    <el-form-item label="主图" prop="mainImageUrl">
      
        <el-upload v-model:file-list="imgList" list-type="picture-card"
          :action= "getAction()"  
          accept="image/*" 
          :headers="getAuthHeaders()"
          :on-preview="handlePictureCardPreview" 
          :on-remove="handleRemove" 
          :before-upload="beforeUpload"        
          :on-success="handleUploadSuccess"
          :on-error="handleUploadError"
          :auto-upload="true">          
          <el-icon >
            <Plus  />
          </el-icon>
        </el-upload>
        
        <el-dialog v-model="dialogVisible">
          <img w-full :src="dialogImageUrl" alt="Preview Image" />
        </el-dialog>
      
    </el-form-item>

    <el-form-item>
      <el-button type="primary" @click="submitForm">提交</el-button>
      <el-button @click="resetForm" v-if="props.isAdd">重置</el-button>
    </el-form-item>
  </el-form>
</template>

<script setup>
import { defineProps, reactive, ref, defineEmits, onMounted } from "vue";
// import { UploadProps, UploadUserFile } from 'element-plus';
import { getAllThemes } from "../../api/theme";
import { addWebsite, updateWebSite,addOrUpdateWebSiteImage} from "../../api/website";
import { $msg_e, $msg_s, $msg_w } from "../../common/msg";
import { BASE_URL } from "../../common/index";
import { formProps } from "element-plus";
const emit = defineEmits(["handleCompleted", "onMountedCompleted"]);
// 表单数据模型
const props = defineProps({
  website: {
    type: Object,
    default: () => {
      return {
        id: "",
        name: "",
        theme: "",
        pageCount: 1,
        price: 0,
        description: "",
        sortNo: 10,
        mainImageUrl: ""
      };
    },
  },
  isAdd: {
    type: Boolean,
    default: true
  }
});
const formRef = ref(null);
const isUpdateImage = ref(false); //是否更新图片
const themeList = ref(["小清新", "小红"]);
const checkThemeList = ref([]); //选中的主题
const imgList = ref([]);

onMounted(() => {
  console.log("websiteForm mounted");
  getAllThemes().then(res => {
    themeList.value = res;
  })
  //延时1秒触发事件
  setTimeout(() => {
    emit("onMountedCompleted");
    console.log("userForm mounted after 1 second", props.isAdd);
  }, 1000);
});


const validateNumber = (rule, value, callback) => {
  if (!value &&value!==0) {
    return callback(new Error('请输入数字'));
  }
  try {
    let newValue = value*1;
    if (!Number.isFinite(newValue)) {
      callback(new Error('请输入有效的数字'));
    } else {
      callback();
    }
  } catch (error) {
    callback(new Error('请输入有效的数字'));
  }
};
// 表单验证规则
const rules = {
  id: [{ required: true, message: "请输入用户编号", trigger: "blur" }],
  name: [{ required: true, message: "请输入用户名", trigger: "blur" }],
  theme: [   {
      validator: (rule, value, callback) => {
        if (value.length === 0) {
          callback(new Error('请选择至少一项兴趣'));
        } else {
          callback();
        }
      },
      trigger: 'change'
    }],
  pageCount: [{ required: true, message: "请输入页数", trigger: "blur" },
  { validator: validateNumber, trigger: 'blur' }],
  price: [{ required: true, message: "请输入价格", trigger: "blur" },
  { validator: validateNumber, trigger: 'blur' }],
};

const checkThemeListChange = (value)=> {
  console.log("checkThemeListChange:", value);
  props.website.theme = value.join(",");
}

// 提交表单方法
const submitForm = async () => {
  //var validated = true;
  // formRef.value.validate((valid) => {
  //   if (!valid) {
  //     validated = false;
  //   } 
  // });
  let validated = await formRef.value.validate();
  if(!validated){
    return;
  }

  if(checkThemeList.value.length==0)
  {
    $msg_w("主题未选择");
    return;
  }
  // 这里可以添加表单提交逻辑，例如调用 API 发送数据至后端服务器
  console.log("Submit:", props.website);
  let params = {id:props.website.id,name: props.website.name,theme:props.website.theme,pageCount:props.website.pageCount,price: props.website.price,description: props.website.description,sortNo: props.website.sortNo,mainImageUrl: props.website.mainImageUrl };
  let res;
  if (props.isAdd) {
    res = await addWebsite(props.website.name,props.website.theme,props.website.pageCount, props.website.price, props.website.description, props.website.sortNo, props.website.mainImageUrl );
    $msg_s("添加成功");
  } else {
    res = await updateWebSite(params);
    $msg_s("修改成功");
  }
  console.log("获得结果res",res)
  if(isUpdateImage.value)
  {
    let imgDetails =[];
    for(let i=0;i<imgList.value.length;i++){
      imgDetails.push({
        WebSiteId:res.id,
        ImageName:"",
        ImagePath:imgList.value[i],
        ImageDescription:"",
        SortNo:i
      })
    }
    let res2 = await addOrUpdateWebSiteImage(res.id,imgDetails)
  }
  resetForm();
  emit("handleCompleted", res);
};


// 重置表单方法
const resetForm = () => {
  props.website.name = "";
  props.website.theme = "";
  props.website.pageCount = 1;
  props.website.description = "";
  props.website.sortNo = 10;
  props.website.price =0;
  checkThemeList.value = [];
  // 清除表单验证错误
  // (this.$refs.userForm as any).clearValidate();
};


const dialogImageUrl = ref('')
const dialogVisible = ref(false)
const getAction = ()=>{
  return `${BASE_URL}/api/Upload/UploadImage?siteId=${props.website.id}}`;
}
const getAuthHeaders=()=> {
      // 这里添加你的认证头部信息
      return {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        'Accept': '*/*',
       // 'Content-Type': 'multipart/form-data; boundary=--------------------------842594888721012071930557'
      };
    }
const beforeUpload=(file)=> {
      
      const isLt2M = file.size / 1024 / 1024 < 2;
      if (!isLt2M) {
        $msg_w('上传头像图片大小不能超过 2MB!');
      }
      return  isLt2M;
    }
const handleRemove = (uploadFile, uploadFiles) => {
  console.log("handleRemove",uploadFile, uploadFiles)
  console.log('imglist',imgList.value)
  isUpdateImage.value = true;
  // imgList.value.push(uploadFile.filePath);
}

const handlePictureCardPreview = (uploadFile) => {
  dialogImageUrl.value = uploadFile.url ?? ""
  dialogVisible.value = true
}

const handleUploadSuccess = (uploadFile) => {
  console.log(`上传成功${uploadFile}`,uploadFile)
  console.log('imglist',imgList.value)
  isUpdateImage.value = true;
  // imgList.value.push(uploadFile.filePath);
}
const handleUploadError = (uploadFile) => {
  console.log(`上传出错${uploadFile}`)
}

//父能访问resetForm,父访问通过ref
defineExpose({
  resetForm
})
</script>

<style>
.el-form-item {
  margin-bottom: 20px;
}
.el-upload--picture-card {
  --el-upload-picture-card-size :80px
}
</style>
