<template>
    <el-select v-model="myVModel"
               :value="myVModel"
               :placeholder="placeholder">
      <el-option v-for="option in this.$store.state.system.sysdict"
                 v-if="option.groupCode === groupCode"
                 :label="option.itemName"
                 :value="option.itemValue"/>
    </el-select>
</template>

<script>
    export default {
      name: "DictSelect",
      data(){
        return {
          myVModel: this.dictValue
        }
      },
      props:{
        groupCode:{type:String,default:""},
        placeholder:{type:String,default:""},
        dictValue:{type:String,default:""},
      },
      watch : {
        'dictValue' : function(newVal, oldVal){
          this.myVModel = newVal;
        },
        myVModel : function(newVal, oldVal){
          this.$emit("update:dictValue", newVal);
        }
      }
    }
</script>

<style scoped>

</style>