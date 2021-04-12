<template>
    <div class="login">
        <el-form class="login-form">
            <h1 class="title">小程序商城</h1>

            <el-alert :title="loginForm.errorMsg" type="error" v-show="loginForm.errorVisible">
            </el-alert>

            <el-form-item >
                <el-input v-model="loginForm.username" type="text"
                          auto-complete="off" placeholder="账号">
                </el-input>
            </el-form-item>
            <el-form-item >
                <el-input v-model="loginForm.password" type="password"
                          auto-complete="off" placeholder="密码">
                </el-input>
            </el-form-item>

            <el-form-item style="width:100%;">
              <el-row :gutter="20">
                <el-col :span="12">
                  <el-button @click="userpwdLogin" size="medium" type="primary" style="width:100%;">
                    <span>登 录</span>
                  </el-button>
                </el-col>
                <el-col :span="12">
                  <el-button @click="userpwdReset" size="medium" type="success" style="width:100%;">
                    <span>重 置</span>
                  </el-button>
                </el-col>
              </el-row>
            </el-form-item>
        </el-form>

        <a href="https://beian.miit.gov.cn/" target="_blank" class="beian">豫ICP备2021006708号</a>
    </div>
</template>

<script>
    import {setJwtToken} from '@/lib/utils'
    import {login} from '@/api/system/sys_user'
    export default {
        name: "login",
        data() {
            return {
                loginForm: {
                    username: "",
                    password: "",
                    errorMsg:"",
                    errorVisible: false
                }
            };
        },
        methods:{
            userpwdLogin(){
                login(this.loginForm.username,
                    this.loginForm.password
                ).then(res =>{
                    setJwtToken(res.data)
                    this.$router.push("home")
                }).catch(err => {
                    this.loginForm.errorMsg = err.message;
                    this.loginForm.errorVisible = true;
                });
            },
            getPassword(){

            },
            userpwdReset(){
                this.loginForm.username = ""
                this.loginForm.password = ""
            }
        }
    }
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
    .login {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100%;
        background-image: url("../assets/img/login-bg.jpg");
        background-size: cover;
    }
    .login-form {
        border-radius: 6px;
        background: #ffffff;
        width: 400px;
        padding: 25px 25px 5px 25px;

        .el-input {
            height: 38px;
            input {
                height: 38px;
            }
        }
    }
    .title{
        text-align:center;
    }
    .beian{
        position: absolute;
        bottom: 15px;
        font-weight: 300;
        cursor: pointer;
        text-transform: uppercase;
        float:none;
        text-align: center;
        display: -webkit-box;
        -webkit-box-orient: horizontal;
        -webkit-box-pack: center;
        -webkit-box-align: center;
    }
</style>