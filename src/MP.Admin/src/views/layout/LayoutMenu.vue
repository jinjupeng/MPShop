<template>
    <el-menu class="el-menu-vertical"
             :default-active="$store.state.maintabs.activeRoute"
             :unique-opened="uniqueOpened"
             :collapse="isMenuCollapse">

        <div class="menu-top">
          <img src="../../assets/logo.png" style="height: 60px;">
          <div class="sys-name">小程序商城</div>
        </div>
        <template v-for="menu in menuList">
            <el-submenu  :index="menu.path" :key="menu.id" v-if="menu.id">
                <template slot="title">
                  <i :class="menu.icon"></i>
                  <span slot="title">{{menu.name}}</span>
                </template>

                <router-link v-for="item in menu.children" :to="item.path">
                  <el-menu-item :index="item.path" :key="item.id">
                    <i :class="item.icon"></i>
                      {{item.name}}
                  </el-menu-item>
                </router-link>
            </el-submenu>
        </template>
    </el-menu>
</template>
<style lang="scss">
    .el-menu-vertical:not(.el-menu--collapse) {
        width: 200px;
        min-height: 400px;
    }
    .el-menu.el-menu--collapse {
        .sys-name{
          display: none;
        }
    }
    .el-menu{
        .menu-top{
          width: 100%;
          height: 60px;
          background-color: #1F9FFF;
          display: flex;
          display: -webkit-flex;
          justify-content: center;
          align-items:center;
          .sys-name{
            color: #FFFFFF;
            font-size: 20px;
          }
        }
        background-color: #2c2c2c;
        .el-submenu__title,.el-submenu__title>i{
            padding-left: 10px !important;
            color: #FFFFFF;
        }
        .el-menu-item{
            padding-left: 40px !important;
            color: #FFFFFF;
        }
        .el-menu-item.is-active,
        .el-menu-item.is-active:hover
        {
            background-color: #1F9FFF;
            color: #FFFFFF
        }
        .el-submenu__title:hover,
        .el-menu-item:hover{
            border-left:  6px solid #FFFFFF;
            background-color: #1F9FFF;
        }
    }

    [class^="el-icon-fa"], [class*="el-icon-fa"] {
      display: inline-block;
      font: normal normal normal 17px/1 FontAwesome!important;
      font-size: inherit;
      text-rendering: auto;
      -webkit-font-smoothing: antialiased;
      -moz-osx-font-smoothing: grayscale;
    }
    $fa-css-prefix: el-icon-fa;
</style>

<script>
    export default {
        name:"LayoutMenu",
        data(){
            return {
                uniqueOpened: true
            }
        },
        props:{
            isMenuCollapse:{
                type: Boolean
            }
        },
        computed:{
            menuList(){
                return this.$store.state.maintabs.menuList;
            }
        }
    }
</script>