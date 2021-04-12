import Vue from 'vue'
import Vuex from 'vuex'
import mutations from './mutations'
import getters from './getters'
import actions from './actions'
import system from './modules/system'
import maintabs from './modules/maintabs'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
  },
  mutations,
  actions,
  getters,
  modules: {
      system,
      maintabs
  }
})
