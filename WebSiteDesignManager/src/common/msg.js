import { ElMessage, ElMessageBox } from "element-plus";

//成功提示
export const $msg_s = (message, duration = 2000) => {
    ElMessage({
        showClose: true,
        message,
        duration,
        type: 'success',
    })
}

//警告提示
export const $msg_w = (message, duration = 2000) => {
    ElMessage({
        showClose: true,
        message,
        duration,
        type: 'warning',
    })
}

//错误提示
export const $msg_e = (message, duration = 2000) => {
    ElMessage({
        showClose: true,
        message,
        duration,
        type: 'error',
    })
}

//一般信息提示
export const $msg_i = (message, duration = 2000) => {
    ElMessage({
        showClose: true,
        message,
        duration,
    })
}

//弹出带确认的 消息提示框
export const $alert = (message, action, title = "提示", confirmButtonText = "OK") => {
    ElMessageBox.alert(message, title, {
        // if you want to disable its autofocus
        // autofocus: false,
        confirmButtonText,
        callback: action
    },
    )
}

export const $confirm = (message, confirmAction, cancelAction, confirmButtonText = "确认", cancelButtonText = "取消", title = "") => {
    ElMessageBox.confirm(
        message,
        title,
        {
            confirmButtonText,
            cancelButtonText,
            type: 'warning',
        }
    )
        .then(confirmAction)
        .catch(cancelAction)
}

export const $prompt=(message,title,confirmAction,cancelAction, confirmButtonText="确认",cancelButtonText="取消")=>{
    ElMessageBox.prompt('Please input your e-mail', 'Tip', {
        confirmButtonText,
        cancelButtonText,
        // inputPattern:
        //   /[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?/,
        // inputErrorMessage: 'Invalid Email',
      })
        .then(({ value }) => {
            confirmAction(value)
        })
        .catch(cancelAction)
}