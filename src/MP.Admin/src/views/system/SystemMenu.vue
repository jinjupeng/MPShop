<template>
  <div>
    <el-card body-style="padding: 0">
      <el-form ref="menuQueryForm" :model="menuQueryForm" label-width="80px">
        <el-row :gutter="20">
          <el-col :span="6">
            <el-form-item label="菜单名称" prop="name">
              <el-input v-model="menuQueryForm.name" placeholder="请输入菜单名称"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="是否禁用" prop="status">
              <dict-select :dictValue.sync="menuQueryForm.status"
                           groupCode="common.status"
                           placeholder="请选择菜单禁用状态">
              </dict-select>
            </el-form-item>
          </el-col>
          <el-col :span="6" :offset="6">
            <el-form-item>
              <el-button type="primary" size="small"
                         @click="submitQueryForm()" icon="el-icon-search">
                查询</el-button>
              <el-button type="primary" size="small" plain
                         @click="resetQueryForm('menuQueryForm')" icon="el-icon-refresh">
                重置</el-button>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>

    <el-card>
      <el-button type="primary" size="small" style="margin: 0 0 10px 20px"
                 icon="el-icon-plus" @click="handleAdd()">新增</el-button>

      <el-table :data="tableData" row-key="id"
                border default-expand-all stripe style="width: 100%;margin-bottom: 20px;">
        <el-table-column prop="menuName" label="菜单名称" width="300" fixed="left"/>
        <el-table-column prop="url" label="访问路径" width="200" align="center"/>
        <el-table-column prop="icon" label="菜单图标" width="200" align="center"/>
        <el-table-column prop="sort" label="排序" width="200" align="center"/>
        <el-table-column prop="status" label="是否禁用" width="" align="center">
          <template slot-scope="scope">
            <el-switch
              active-text ="是"
              inactive-text = "否"
              v-model="scope.row.status"
              @change=changeStatus(scope.$index,scope.row) >
            </el-switch>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="90" fixed="right">
          <template slot-scope="scope">
            <el-button size="mini" type="primary" icon="el-icon-edit" circle
                       @click="handleEdit(scope.$index, scope.row)"/>
            <el-button size="mini" type="danger" icon="el-icon-delete" circle
                       @click="handleDelete(scope.$index, scope.row)"/>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog :title="dialogTitle" :visible.sync="dialogFormVisible"
               :before-close="beforeDialogClose">
      <el-form :model="dialogForm" ref="dialogForm"
               :rules="dialogFormRules" label-width="80px">
        <el-row :gutter="20">
          <el-col :span="24">
            <el-form-item label="上级菜单" prop="menuPid" >
              <el-tree-select
                :elTreeProps="elTreeProps"
                :elTreeData="tableData"
                :defaultSelectedId="dialogForm.menuPid"
                :disabled="elTreeDisabled"
                @handleTreeSelected="handleTreeSelected($event)"
                @validateSelectTree="validateSelectTree"/>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="菜单名称" prop="menuName">
              <el-input v-model="dialogForm.menuName" autocomplete="off"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="访问路径" prop="url">
              <el-input v-model="dialogForm.url" autocomplete="off"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="菜单图标" prop="icon">
              <el-input v-model="dialogForm.icon" autocomplete="off"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="排序序号" prop="sort">
              <el-input-number v-model="dialogForm.sort" :min="1"  label="排序序号"></el-input-number>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="submitDialogForm" size="mini" type="primary">确 定</el-button>
        <el-button @click="handleCloseDialog" size="mini">取 消</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
  import {getMenuTree,updateMenu,addMenu,deleteMenu,changeMenuStatus}
  from '../../api/system/sys_menu'
  import ElTreeSelect from "@/components/TreeSelect";
  import DictSelect from "@/components/DictSelect";

  export default {
    name: "SystemMenu",
    components: {ElTreeSelect,DictSelect},
    data() {
      return {
        tableData: [],
        menuQueryForm: {
          name: "",
          status: null
        },
        elTreeDisabled:false,
        elTreeProps:{         // el-tree-select配置项（必选）
          value: 'id',
          label: 'menuName',
          children: 'children',
        },
        dialogFormVisible: false,
        dialogTitle:"",
        dialogForm: {
          id: null,
          menuPid: null, // el-tree-select初始ID（可选）
          menuName: '',
          url: '',
          icon: '',
          sort: ''
        },
        dialogFormRules: {
          menuPid: [
            {required: true, message: '请选择上级菜单',trigger:'blur'},
          ],
          menuName: [
            {required: true, message: '请输入菜单名称', trigger: 'blur'},
          ],
          sort: [
            {required: true, message: '请输入当前菜单在同级菜单内的排序序号', trigger: 'blur'},
          ]
        }
      }
    },
    methods: {
      submitQueryForm() {
          getMenuTree({name: this.menuQueryForm.name, status: this.menuQueryForm.status})
          .then(res => {
            this.setData(res)
          })
      },
      resetQueryForm(formName) {
        this.$refs[formName].resetFields();
        this.submitQueryForm(formName)
      },
      changeStatus(index,row){
        changeMenuStatus(row.id,row.status).then(res => {
          if(res.isok){ //如果请求成功，给出成功信息提示
            //res.data=controller返回的数据
            this.$message({message: res.data, type: 'success'});
          }
        })
      },
      handleAdd(){
        this.elTreeDisabled = false;
        this.dialogFormVisible = true;
        this.dialogTitle = "新增菜单项"
        this.resetDialogForm()
      },
      handleEdit(index, row) {
        this.elTreeDisabled = true;
        this.dialogFormVisible = true;
        this.dialogTitle = "修改菜单项"
        this.resetDialogForm()
        this.dialogForm = {...row}
      },
      resetDialogForm(){
        this.dialogForm.id = null
        this.dialogForm.menuPid = null
        this.dialogForm.menuName = ''
        this.dialogForm.url = ''
        this.dialogForm.icon = ''
        this.dialogForm.sort = ''
      },
      submitDialogForm(){
        this.$refs.dialogForm.validate((valid) => {
          if (valid) {
            this.$confirm("确定提交数据么?")
              .then(_ => {
                if(this.elTreeDisabled){
                  updateMenu(this.dialogForm)
                    .then(res => {
                      this.$message({message: res.data, type: 'success'});
                      this.submitQueryForm();//修改之后，重新查询table
                      this.handleCloseDialog();
                    })
                }else {
                  addMenu(this.dialogForm).then(res => {
                    this.$message({message: res.data, type: 'success'});
                    this.submitQueryForm();//新增之后，重新查询table
                    this.handleCloseDialog();
                  })
                }
                //取消新增或修改也要重置表单
              }).catch(_ => {this.handleCloseDialog();});
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
        this.$refs['dialogForm'].resetFields();
        this.dialogFormVisible = false;
      },
      beforeDialogClose(done){
        this.$confirm('确认关闭？')
          .then(_ => {
            this.$refs['dialogForm'].resetFields();
            done();
          }).catch(_ => {});
      },
      handleDelete(index, row) {
        this.$confirm("确定删除["+row.menuName+"]?")
        .then(_ => {
          this.dialogForm = {...row}
          deleteMenu(this.dialogForm)
          .then(res => {
            this.submitQueryForm();//删除之后，重新查询table
            this.$message({message: res.data, type: 'success'});
          }).catch(err => {
            this.$message({message: err.message, type: 'error'});
          })
        });
      },
      handleTreeSelected(value){
        this.dialogForm.menuPid = value
        this.$refs.dialogForm.validateField("menuPid");
      },
      validateSelectTree(){
        this.$refs.dialogForm.validateField("menuPid");
      },
      setData(menuTree) {
        if (menuTree.isok) {
          this.tableData = menuTree.data
        }
      }
    },
    beforeRouteEnter(to, from, next) {
      getMenuTree({name: "", status: null}).then(res => {
        next(vm => vm.setData(res))
      })
    }
  }
</script>

<style scoped>
  .el-form {
    margin-top: 20px;
  }
</style>