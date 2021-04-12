<template>
</template>
<script>
  //"增删改"通用mixin
  export default {
    name: "MixinCUD",
    data() {
      return {
        //mixin里面尽量不定义数据,在引用mixin的组件中定义如下数据
        //dialogFormVisible: false,
        //dialogTitle:"弹出框表单标题",
        //dialogForm: { 这里写数据表单属性},
        //dialogFormRules: { 这里写数据表单校验规则}
        //dialogRefName:"这里与弹出框数据表单的ref属性一致"
        //queryFormRefName:"这里与查询表单的ref属性一致"
      }
    },
    methods: {
      submitQueryForm() {
        this.getData()
      },
      resetQueryForm() {
        this.$refs[this.queryFormRefName].resetFields();
        this.submitQueryForm()
      },
      handleAdd(dialogTitle){
        this.dialogFormVisible = true;
        this.dialogTitle = dialogTitle
        this.resetDialogForm()
      },
      handleEdit(index, row,dialogTitle) {
        this.dialogFormVisible = true;
        this.dialogTitle = dialogTitle
        this.resetDialogForm()
        this.dialogForm = {...row}
      },
      resetDialogForm(){
        for(let item in this.dialogForm){
          if (typeof this.dialogForm[item]==='string'){
            this.dialogForm[item]='';
          } else if (this.dialogForm[item] instanceof Array) {
            this.dialogForm[item]=[];
          } else{
            this.dialogForm[item]=null
          }
        }
      },
      submitDialogForm(){
        this.$refs[this.dialogRefName].validate((valid) => {
          if (valid) {
            this.$confirm("确定提交数据么?")
              .then(_ => {
                if(this.dialogForm.id){ //有id是更新数据，没有id是新增数据
                  this.updateData();
                }else {
                  this.addData();
                }
                //取消新增或修改也要重置表单
              }).catch(err => {this.handleCloseDialog();});
          } else {
            return false;
          }
        });
      },
      handleCloseDialog(){
        //resetFields就是一个坑，有两个作用
        //1.重置的值不是空的，而是第一次被赋予的值。
        //第一次dialogForm赋空值，后续才能重置为空值。
        //这就是我们在新增修改打开弹出框操作的时候，调用resetDialogFrom清空数据的原因。
        //2.清空校验结果
        this.$refs[this.dialogRefName].resetFields();
        this.dialogFormVisible = false;
      },
      beforeDialogClose(done){
        this.$confirm('确认关闭？')
          .then(_ => {
            this.$refs[this.dialogRefName].resetFields();
            done();
          }).catch(_ => {});
      },
      handleDelete(index, row) {
        this.deleteData(row)
      }
    }
  }
</script>
<style scoped>
</style>