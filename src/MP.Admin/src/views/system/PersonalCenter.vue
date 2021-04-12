<template>
    <div>
      <el-row>
        <el-col :span="8">
          <el-card class="box-card">
            <div slot="header" class="clearfix">
              <span>个人中心</span>
            </div>
            <el-row  style="margin: 5px">
              <el-col :span="12">用户名：</el-col>
              <el-col :span="12">{{username}}</el-col>
            </el-row>
            <el-row  style="margin: 5px">
              <el-col :span="12">电  话： </el-col>
              <el-col :span="12">{{phone}}</el-col>
            </el-row>
            <el-row  style="margin: 5px">
              <el-col :span="12">email：</el-col>
              <el-col :span="12">{{email}}</el-col>
            </el-row>
          </el-card>
        </el-col>
        <el-col :span="16">
          <el-card>
            <h3>修改密码</h3>
            <el-form :model="pwdForm" status-icon :rules="pwdFormRules" ref="pwdForm" label-width="100px">
              <el-form-item label="原密码" prop="oldPass">
                <el-input type="password" v-model="pwdForm.oldPass" autocomplete="off"></el-input>
              </el-form-item>
              <el-form-item label="新密码" prop="password">
                <el-input type="password" v-model="pwdForm.password" autocomplete="off"></el-input>
              </el-form-item>
              <el-form-item label="确认密码" prop="checkPass">
                <el-input type="password" v-model="pwdForm.checkPass" autocomplete="off"></el-input>
              </el-form-item>
              <el-form-item>
                <el-button type="primary" @click="submitPwdForm('pwdForm')">提交</el-button>
                <el-button @click="resetPwdForm('pwdForm')">重置</el-button>
              </el-form-item>
            </el-form>
          </el-card>
        </el-col>
      </el-row>
    </div>
</template>

<script>
    import {getUserInfo,changeUserPwd} from '@/api/system/sys_user';

    export default {
        name: "PersonalCenter",
        data(){
          let validatePass2 = (rule, value, callback) => {
            if (value === '') {
              callback(new Error('请再次输入密码'));
            } else if (value !== this.pwdForm.password) {
              callback(new Error('两次输入密码不一致!'));
            } else {
              callback();
            }
          };
          return {
            username: "",
            phone: "",
            email: "",
            pwdForm: {
              oldPass:'',
              password: '',
              checkPass: ''
            },
            pwdFormRules: {
              oldPass: [
                { required: true, message:"原密码必须填写",trigger: 'blur' }
              ],
              password: [
                { required: true, message: '请输入密码', trigger: 'blur' },
                { pattern: /^(?![a-zA-Z]+$)(?![A-Z0-9]+$)(?![A-Z\W_]+$)(?![a-z0-9]+$)(?![a-z\W_]+$)(?![0-9\W_]+$)[a-zA-Z0-9\W_]{8,30}$/,
                  message: '密码需包含数字、大小写字母、特殊符号，长度为 8 - 30位' }
              ],
              checkPass: [
                { required: true, validator: validatePass2, trigger: 'blur' }
              ]
            }
          }
        },
        beforeRouteEnter (to, from, next) {
            getUserInfo().then(res => {
                next(vm => vm.setData(res))
            })
        },
        methods: {
          setData(userinfo){
              if(userinfo.isok){
                  this.username = userinfo.data.username
                  this.phone = userinfo.data.phone
                  this.email = userinfo.data.email
              }
          },
          submitPwdForm(formName){

            this.$refs[formName].validate((valid) => {
              if (valid) {
                this.$confirm("确定修改密码么?")
                .then(_ => {
                  changeUserPwd(this.pwdForm.oldPass,this.pwdForm.password)
                  .then(res=>{
                    this.$message({message: res.data, type: 'success'})
                    this.resetPwdForm(formName)
                  }).catch(err =>{
                    this.$message.error( err.message)
                  })
                });
              }
            });
          },
          resetPwdForm(formName){
            this.$refs[formName].resetFields();
          }
        }
    }
</script>

<style scoped>
    .clearfix:before,
    .clearfix:after {
        display: table;
        content: "";
    }
    .clearfix:after {
        clear: both
    }
</style>