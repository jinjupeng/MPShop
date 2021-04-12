<template>
  <div>
    <el-card body-style="padding: 0">
      <el-form ref="roleQueryForm" :model="roleQueryForm" label-width="80px">
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="角色" prop="roleLike">
              <el-input v-model="roleQueryForm.roleLike"
                        placeholder="请输入角色名称或角色编码或角色描述"/>
            </el-form-item>
          </el-col>
          <el-col :span="6" :offset="6">
            <el-form-item>
              <el-button type="primary" size="small"
                         @click="submitQueryForm()" icon="el-icon-search">
                查询</el-button>
              <el-button type="primary" size="small" plain
                         @click="resetQueryForm('roleQueryForm')" icon="el-icon-refresh">
                重置</el-button>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>

    <el-card>
      <el-button type="primary" size="small" style="margin: 0 0 10px 20px"
                 icon="el-icon-plus" @click="handleAdd()">新增</el-button>

      <el-table :data="tableData" border default-expand-all stripe style="width: 100%;margin-bottom: 20px;">
        <el-table-column prop="roleName" label="角色名称" width="200" align="center"/>
        <el-table-column prop="roleCode" label="角色编码" width="200" align="center"/>
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
        <el-table-column prop="roleDesc" label="角色描述" width="" align="center"/>
        <el-table-column label="操作" width="200" align="center">
          <template slot-scope="scope">
            <el-button size="mini" type="primary" icon="el-icon-edit" circle
                       @click="handleEdit(scope.$index, scope.row)"/>
            <el-button size="mini" type="danger" icon="el-icon-delete" circle
                       @click="handleDelete(scope.$index, scope.row)"/>
            <el-button size="mini" type="success"
                       @click="assignPerm(scope.$index, scope.row)">
              分配权限
            </el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog :title="dialogTitle" :visible.sync="dialogFormVisible"
               :before-close="beforeDialogClose">
      <el-form :model="dialogForm" ref="dialogForm"
               :rules="dialogFormRules" label-width="80px">

        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="角色名称" prop="roleName">
              <el-input v-model="dialogForm.roleName" autocomplete="off"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="角色编码" prop="roleCode">
              <el-input v-model="dialogForm.roleCode" autocomplete="off"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="角色描述" prop="roleDesc">
              <el-input v-model="dialogForm.roleDesc"  label="角色描述"></el-input>
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

    <el-dialog :title="permDialogTitle" :visible.sync="permDialogVisible">
      <el-row>
        <el-col :span="12">
          <multi-tree
            :data="menuData"
            :labelPropName="menuLabelPropName"
            :buttonName="menuButtonName"
            :defaultExpandedKeys="menuDefaultExpandedKeys"
            :defaultCheckedKeys="menuDefaultCheckedKeys"
            @handleCheckedKeys="handleMenuCheckedKeys"></multi-tree>
        </el-col>
        <el-col :span="12">
          <multi-tree
            :data="apiData"
            :labelPropName="apiLabelPropName"
            :buttonName="apiButtonName"
            :defaultExpandedKeys="apiDefaultExpandedKeys"
            :defaultCheckedKeys="apiDefaultCheckedKeys"
            @handleCheckedKeys="handleApiCheckedKeys"></multi-tree>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>

<script>
  import {getRoles,updateRole,addRole,deleteRole,changeRoleStatus} from '@/api/system/sys_role'
  import {getMenuCheckedTree,saveMenuCheckedKeys} from '@/api/system/sys_menu'
  import {getApiCheckedTree,saveApiCheckedKeys} from '@/api/system/sys_api'
  import axios from 'axios'
  import MultiTree from "../../components/MultiTree";

  export default {
    name: "SystemRole",
    components: {MultiTree},
    data() {
      return {
        tableData: [],
        roleQueryForm:{
          roleLike: ""
        },
        dialogFormVisible: false,
        dialogTitle:"",
        dialogForm: {
          id: '',
          roleName: '',
          roleCode: '',
          roleDesc: '',
          sort: ''
        },
        dialogFormRules: {
          roleName: [
            {required: true, message: '请输入角色名称', trigger: 'blur'},
          ],
          roleCode: [
            {required: true, message: '请输入角色编码', trigger: 'blur'},
            {pattern: /^[A-Za-z0-9]+$/, message: '只能包含英文、数字', trigger: 'blur'}
          ],
          roleDesc: [
            {required: true, message: '请输入角色描述信息', trigger: 'blur'},
          ]
        },
        handlingRoleId: null,
        permDialogVisible:false,
        permDialogTitle:"",
        menuData:[],
        menuLabelPropName:"menuName",
        menuButtonName:"保存菜单查看权限",
        menuDefaultExpandedKeys:[],
        menuDefaultCheckedKeys:[],
        apiData:[],
        apiLabelPropName:"apiName",
        apiButtonName:"保存接口访问权限",
        apiDefaultExpandedKeys:[],
        apiDefaultCheckedKeys:[]
      }
    },
    methods: {
      submitQueryForm() {
        getRoles(this.roleQueryForm.roleLike)
          .then(res => {
            this.setData(res)
          })
      },
      resetQueryForm(formName) {
        this.$refs[formName].resetFields();
        this.submitQueryForm(formName)
      },
      changeStatus(index,row){
        changeRoleStatus(row.id,row.status).then(res => {
          if(res.isok){ //如果请求成功，给出成功信息提示
            //res.data=controller返回的数据
            this.$message({message: res.data, type: 'success'});
          }
        })
      },
      handleAdd(){
        this.dialogFormVisible = true;
        this.dialogTitle = "新增角色"
        this.resetDialogForm()
      },
      handleEdit(index, row) {
        this.dialogFormVisible = true;
        this.dialogTitle = "修改角色"
        this.resetDialogForm()
        this.dialogForm = {...row}
      },
      resetDialogForm(){
        this.dialogForm.id = null
        this.dialogForm.roleName = ''
        this.dialogForm.roleCode = ''
        this.dialogForm.roleDesc = ''
        this.dialogForm.sort = ''
      },
      submitDialogForm(){
        this.$refs.dialogForm.validate((valid) => {
          if (valid) {
            this.$confirm("确定提交数据么?")
              .then(_ => {
                if(this.dialogForm.id){ //有id是更新数据，没有id是新增数据
                  updateRole(this.dialogForm)
                    .then(res => {
                      this.$message({message: res.data, type: 'success'});
                      this.submitQueryForm();//修改之后，重新查询table
                      this.handleCloseDialog();
                    })
                }else {
                  addRole(this.dialogForm).then(res => {
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
        this.$confirm("确定删除["+row.roleName+"]?")
          .then(_ => {
            deleteRole(row.id)
              .then(res => {
                this.submitQueryForm();//删除之后，重新查询table
                this.$message({message: res.data, type: 'success'});
              }).catch(err => {
              this.$message({message: err.message, type: 'error'});
            })
          });
      },
      assignPerm(index, row){
        this.handlingRoleId = row.id
        let _this = this
        axios.all([getMenuCheckedTree(row.id), getApiCheckedTree(row.id)])
        .then(axios.spread(function (res1, res2) {
          // 两个请求都执行完成后，进入该函数
          _this.menuData = res1.data.tree
          _this.menuDefaultExpandedKeys = res1.data.expandedKeys
          _this.menuDefaultCheckedKeys = res1.data.checkedKeys
          _this.apiData = res2.data.tree
          _this.apiDefaultExpandedKeys = res2.data.expandedKeys
          _this.apiDefaultCheckedKeys = res2.data.checkedKeys
          _this.permDialogVisible = true;
          _this.permDialogTitle = row.roleName + "角色:权限分配"
        }));
      },
      handleMenuCheckedKeys(checkedKeys){
        saveMenuCheckedKeys(this.handlingRoleId,checkedKeys)
          .then(res => {
            this.$message({message: res.data, type: 'success'});
          })
      },
      handleApiCheckedKeys(checkedKeys){
        saveApiCheckedKeys(this.handlingRoleId,checkedKeys)
          .then(res => {
            this.$message({message: res.data, type: 'success'});
          })
      },
      setData(roles) {
        if (roles.isok) {
          this.tableData = roles.data
        }
      }
    },
    beforeRouteEnter(to, from, next) {
      getRoles("").then(res => {
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