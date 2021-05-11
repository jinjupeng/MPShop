// miniprogram/pages/index/index.js
Page({

  /**
   * 页面的初始数据
   */
  data: {
    swiperCurrent: 0, // 当前所在页面的index
    indicatorDots: true, // 是否显示面板指示点
    autoplay: true, // 是否自动切换
    interval: 3000, // 自动切换事件间隔,3s
    duration: 800, // 滑动动画时长1s
    circular: true, // 是否采用衔接滑动
    imgUrls: [ // 图片路径（可以是本地路径，也可以是图片链接）
    ],
    links: [ // 点击图片之后跳转的路径
      '../user/user',
      '../user/user',
      '../user/user'
    ]
  },

  //轮播图的切换事件
  swiperChange: function (e) {
    this.setData({
      swiperCurrent: e.detail.current
    })
  },
  //点击指示点切换事件
  chuangEvent: function (e) {
    this.setData({
      swiperCurrent: e.currentTarget.id
    })
  },
  //点击图片触发事件
  swipclick: function (e) {
    console.log(this.data.swiperCurrent);
    wx.switchTab({
      url: this.data.links[this.data.swiperCurrent]
    })
  },
  /**
   * 生命周期函数--监听页面加载
   * 获取轮播图数据
   */
  async onLoad() {
    let imgArr = [];
    let res = await wx.wxp.request({
      url:'http://localhost:5000/api/goodsinfo/GetCarousel'
    })
    if (res.data.msg == 'ok'){
      let dataList = res.data.data;
      for(let i = 0; i < dataList.length; i++) {
        imgArr.push(dataList[i].content)
      }
      this.setData({
        imgUrls: imgArr
      })
    }
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})