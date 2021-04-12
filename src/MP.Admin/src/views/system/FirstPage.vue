<template>
    <div style="margin-top: 150px;text-align: center;">
      <h2>小程序商城首页</h2>
    </div>
</template>

<script>
    import {pwdIsDefault} from '@/api/system/sys_user'
    export default {
      name: "FirstPage",
      created:function(){
        pwdIsDefault().then(res => {
          if(res.isok && res.data){//表示正在使用默认密码
            this.$confirm("您仍在使用系统初始化密码，建议您去修改", '提示', {
              confirmButtonText: '现在就去',
              cancelButtonText: '以后再说',
              type: 'warning'
            })
            .then(_ => {
              this.$router.push({name:"personal"})
            });
          }
        })
      }
    }
</script>

<style scoped>

</style>