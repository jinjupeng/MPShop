const path = require('path')
const resolve = dir => path.join(__dirname, dir)

module.exports = {
  //publicPath: process.env.NODE_ENV === 'production' ? "./" : "/",
  //chainWebpack: config => {
  //  config.resolve.alias
  //    .set('@', resolve('src'))
  //},
  // 配置代理
  devServer: {
    proxy: {
      '/api': {
        target: 'https://lintcoder.cn:5001', // 要访问的接口域名
        changeOrigin: true,//开启代理：在本地会创建一个虚拟服务端，然后发送请求的数据，并同时接收请求的数据，这样服务端和服务端进行数据的交互就不会有跨域问题
        pathRewrite: {
          '/api': ''//这里理解成用'/api'代替target里面的地址,比如我要调用'http://40.00.100.100:3002/user/add'，直接写'/api/user/add'即可
        }
      }
    }
  }
}