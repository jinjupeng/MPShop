<template>
  <el-container>
    <el-aside style="width: auto" class="scroll-view">
      <LayoutMenu :isMenuCollapse="isMenuCollapse"></LayoutMenu>
    </el-aside>

    <el-container>
      <el-header>
        <i :class="getMenuCollapse()" @click="handleCollapse"></i>

        <div class="header-right">
          <a
            style="align-self: center"
            target="_blank"
            href="https://github.com/jinjupeng/Item.ApiServer"
          >
            <img class="right-item" src="../assets/font/doc.svg" />
          </a>
          <a
            style="align-self: center"
            target="_blank"
            href="https://github.com/jinjupeng/Item.Vue"
          >
            <img class="right-item" src="../assets/font/github.svg" />
          </a>
          <el-dropdown>
            <span class="el-dropdown-link">
              <el-avatar style="float: left"> {{ username }} </el-avatar>
              <i
                class="el-icon-arrow-down el-icon--right"
                style="padding-top: 20px"
              ></i>
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item @click.native="personal"
                >个人中心</el-dropdown-item
              >
              <el-dropdown-item divided @click.native="logout"
                >退出登录</el-dropdown-item
              >
            </el-dropdown-menu>
          </el-dropdown>
        </div>
      </el-header>

      <el-main class="scroll-view">
        <el-tabs
          v-model="$store.state.maintabs.activeRoute"
          type="border-card"
          closable
          @tab-click="clickTab"
          @tab-remove="removeTab"
        >
          <el-tab-pane
            v-for="item in $store.state.maintabs.maintabs"
            :key="item.route"
            :label="item.name"
            :name="item.route"
          ></el-tab-pane>
        </el-tabs>
        <div style="margin: 50px 0 0 10px">
          <router-view></router-view>
        </div>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import LayoutMenu from "./layout/LayoutMenu";
import { getTokenUser, setJwtToken } from "@/lib/utils";

export default {
  name: "home",
  components: {
    LayoutMenu,
  },
  data() {
    return {
      isMenuCollapse: false,
    };
  },
  methods: {
    handleCollapse() {
      this.isMenuCollapse = !this.isMenuCollapse;
    },
    getMenuCollapse() {
      return {
        "el-icon-s-fold": !this.isMenuCollapse,
        "el-icon-s-unfold": this.isMenuCollapse,
      };
    },
    removeTab(targetName) {
      if (targetName !== "/home/firstpage") {
        this.$store.commit("removeTab", targetName);
        this.$router.push(
          this.$store.state.maintabs.maintabs[
            this.$store.state.maintabs.maintabs.length - 1
          ].route
        );
      }
    },
    clickTab(tab) {
      this.$router.push(tab.$options.propsData.name);
    },
    logout() {
      this.$router.push("/");
      sessionStorage.clear();
      window.location.reload(); //刷一下可以清空vuex-store
    },
    personal() {
      this.$router.push({ name: "personal" });
    },
  },
  computed: {
    username() {
      return getTokenUser();
    },
  },
};
</script>

<style lang="scss">
#app > .el-container {
  padding: 0;
  margin: 0;
  height: 100%;
}

.header-right {
  float: right;
  display: flex;
  height: 100%;
  flex-direction: row;
  .right-item {
    height: 25px;
    color: #ffffff;
    margin-right: 20px;
    align-self: center;
  }
  .el-dropdown {
    flex: 1;
    text-align: right;
    align-self: center;
    .el-avatar {
      color: #1f9fff;
      background-color: #ffffff;
    }
  }
}
.el-header {
  background-color: #1f9fff;
  .el-icon-s-fold,
  .el-icon-s-unfold {
    font-size: 25px;
    color: #ffffff;
    line-height: 60px;
  }
}

.el-aside {
  background-color: #2c2c2c;
}

.el-main {
  background-color: #ffffff;
  padding: 0 10px 10px 0;
  position: relative;
}

.el-tabs {
  position: fixed;
  border: 0;
  width: 98%;
  z-index: 1000;
}

//当左侧菜单长度超出屏幕，可以滚动，
//但是不显示滚动条,这样更美观
.scroll-view::-webkit-scrollbar {
  display: none;
}

//不显示tab-content，内容区域我们使用router-view自己定义过
.el-tabs--border-card > .el-tabs__content {
  padding: 0;
}

.el-tabs--border-card > .el-tabs__header .el-tabs__item.is-active {
  color: #eff5ff;
  background-color: #303133;
}

//第一个标签首页，不显示x关闭
.el-tabs__nav > .el-tabs__item:nth-child(1) span {
  display: none;
}
</style>
