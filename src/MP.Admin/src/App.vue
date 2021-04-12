<template>
  <div id="app">
    <router-view/>
  </div>
</template>
<script>
    export default {
        name: 'app',
        components: {}
    }
</script>
<style lang="scss">
html,body,#app {
  height: 100%;
  padding: 0;
  margin: 0;
  font-family: "Helvetica Neue", Helvetica, "PingFang SC", "Hiragino Sans GB", "Microsoft YaHei", "微软雅黑", Arial, sans-serif;
}
a{
  text-decoration:none;
}
#nprogress .bar {
    background: black !important; //自定义颜色
    height:5px;
}
</style>
