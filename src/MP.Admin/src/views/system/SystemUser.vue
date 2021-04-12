<template>
  <el-row>
    <el-col :span="4">
      <el-input
        placeholder="输入关键字进行过滤"
        v-model="filterText">
      </el-input>
      <el-tree
        class="filter-tree"
        :data="orgData"
        :props="defaultProps"
        default-expand-all
        :expand-on-click-node="false"
        :filter-node-method="filterOrg"
        @node-click="orgNodeClick"
        :highlight-current="true"
        ref="orgQueryTree">
      </el-tree>
    </el-col>

    <el-col :span="20">
      <div>
        <el-card body-style="padding: 0">
          <el-form ref="userQueryForm" :model="userQueryForm" label-width="80px">
            <el-row :gutter="20">
              <el-col :span="6">
                <el-form-item label="用户名称" prop="username">
                  <el-input v-model="userQueryForm.username" placeholder="请输入用户名称"/>
                </el-form-item>
              </el-col>
              <el-col :span="6">
                <el-form-item label="联系电话" prop="phone">
                  <el-input v-model="userQueryForm.phone" placeholder="请输入联系电话"/>
                </el-form-item>
              </el-col>
              <el-col :span="6">
                <el-form-item label="用户邮箱" prop="email">
                  <el-input v-model="userQueryForm.email" placeholder="请输入用户邮箱"/>
                </el-form-item>
              </el-col>
            </el-row>

            <el-row :gutter="20">
              <el-col :span="6">
                <el-form-item label="用户状态" prop="enabled">
                  <dict-select :dictValue.sync="userQueryForm.enabled"
                               groupCode="sysuser.enabled"
                               placeholder="请选择用户状态">
                  </dict-select>
                </el-form-item>
              </el-col>
              <el-col :span="6">
                <el-form-item label="创建时间" prop="timeRange">
                  <el-date-picker
                    v-model="userQueryForm.timeRange"
                    type="daterange"
                    value-format="yyyy-MM-dd hh:mm:ss"
                    range-separator="至"
                    start-placeholder="开始日期"
                    end-placeholder="结束日期">
                  </el-date-picker>
                </el-form-item>
              </el-col>
              <el-col :span="6" :offset="6" padding="0 20px">
                  <el-button type="primary" size="small"
                             @click="submitQueryForm()" icon="el-icon-search">
                    查询</el-button>
                  <el-button type="primary" size="small" plain
                             @click="resetQueryForm('userQueryForm')" icon="el-icon-refresh">
                    重置</el-button>
              </el-col>
            </el-row>
          </el-form>
        </el-card>

        <el-card>
          <el-button type="primary" size="small" style="margin: 0 0 10px 20px"
                     icon="el-icon-plus" @click="handleAdd('新增用户')">新增</el-button>

          <el-table :data="tableData" border default-expand-all stripe style="width: 100%;margin-bottom: 20px;">
            <el-table-column prop="username" label="用户名称" width="100" align="center"/>
            <el-table-column prop="orgName" label="所在组织" width="200" align="center"/>
            <el-table-column prop="enabled" label="用户状态" width="150" align="center">
              <template slot-scope="scope">
                <el-switch
                  active-text ="激活"
                  inactive-text = "禁用"
                  v-model="scope.row.enabled"
                  @change=changeEnabled(scope.$index,scope.row)
                >
                </el-switch>
              </template>
            </el-table-column>
            <el-table-column prop="phone" label="联系电话" width="120" align="center"/>
            <el-table-column prop="email" label="用户邮箱" width="150" align="center"/>
            <el-table-column prop="createTime" label="创建时间" width="200" align="center" />
            <el-table-column label="操作" width="300" align="center" fixed="right">
              <template slot-scope="scope">
                <el-button size="mini" type="primary" icon="el-icon-edit" circle
                           @click="handleEdit(scope.$index, scope.row,'修改用户')"/>
                <el-button size="mini" type="danger" icon="el-icon-delete" circle
                           @click="handleDelete(scope.$index, scope.row)"/>
                <el-button size="mini" type="success"
                           @click="resetPwd(scope.$index, scope.row)">
                  重置密码
                </el-button>
                <el-button size="mini" type="success"
                           @click="assignRole(scope.$index, scope.row)">
                  分配角色
                </el-button>
              </template>
            </el-table-column>
          </el-table>
          <el-pagination
            :page-sizes="[20, 50, 100, 200]"
            layout="total, sizes, prev, pager, next, jumper"
            :current-page="pagination.pageNum"
            :page-size="pagination.pageSize"
            :total="pagination.total"
            @size-change="handlePageSizeChange"
            @current-change="handlePageNumChange"
            background
            style="float: right;margin-bottom: 10px">
          </el-pagination>
        </el-card>

        <el-dialog :title="dialogTitle" :visible.sync="dialogFormVisible"
                   :before-close="beforeDialogClose">
          <el-alert
            :title="dialogAlertInfo"
            type="error" :closable="false">
          </el-alert>
          <el-form :model="dialogForm" ref="dialogForm"
                   :rules="dialogFormRules" label-width="80px">

            <el-row :gutter="20">
              <el-col :span="12">
                <el-form-item label="用户名称" prop="username">
                  <el-input v-model="dialogForm.username" autocomplete="off"></el-input>
                </el-form-item>
              </el-col>

              <el-col :span="12">
                <el-form-item label="用户组织" prop="orgId">
                    <el-tree-select
                      :elTreeProps="elTreeProps"
                      :elTreeData="orgData"
                      :defaultSelectedId="dialogForm.orgId"
                      :disabled="elTreeDisabled"
                      @handleTreeSelected="handleTreeSelected($event)"
                      @validateSelectTree="validateSelectTree">
                    </el-tree-select>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row :gutter="20">
              <el-col :span="12">
                <el-form-item label="用户邮箱" prop="email">
                  <el-input v-model="dialogForm.email"  label="角色描述"></el-input>
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="联系电话" prop="phone">
                  <el-input v-model="dialogForm.phone" autocomplete="off"></el-input>
                </el-form-item>
              </el-col>

            </el-row>
          </el-form>
          <div slot="footer" class="dialog-footer">
            <el-button @click="submitDialogForm()" size="mini" type="primary">确 定</el-button>
            <el-button @click="handleCloseDialog" size="mini">取 消</el-button>
          </div>
        </el-dialog>
        <el-dialog :title="roleDialogTitle" :visible.sync="roleDialogVisible">
          <el-button type="primary"
                     size="small"
                     style="float: right"
                     @click="saveUserRoles">
            保存角色
          </el-button>
          <el-transfer
            v-model="checkedRoleIds"
            :props="roleDataProp"
            :data="roleDatas"
            :titles="['备选角色', '已选角色']">
          </el-transfer>
        </el-dialog>
      </div>
    </el-col>
  </el-row>
</template>

<script>
  import {getUsers,updateUser,addUser,deleteUser,resetUserPwd,changeEnabled}
  from '@/api/system/sys_user'
  import {getCheckedRoles,saveCheckedUserRoles} from '@/api/system/sys_role'
  import {getOrgTree} from '@/api/system/sys_org'
  import axios from 'axios'
  import MixinCUD from '@/components/MixinCUD'
  import ElTreeSelect from "@/components/TreeSelect";
  import DictSelect from "@/components/DictSelect";

  export default {
    name: "SystemUser",
    mixins: [MixinCUD],
    components:{ElTreeSelect,DictSelect},
    data() {
      return {
        tableData: [],
        queryFormRefName:"userQueryForm",
        userQueryForm:{
          username: "",
          phone:"",
          enabled:"",
          email:"",
          orgId:null,
          timeRange: ["",""]
        },
        pagination:{
          pageNum: 1,
          pageSize: 20,
          total: null
        },
        filterText: '',
        orgData:[],
        defaultProps: {
          children: 'children',
          label: 'orgName'
        },
        dialogFormVisible: false,
        dialogTitle:"",
        dialogRefName:"dialogForm",
        dialogForm: {
          id: null,
          username: "",
          phone:"",
          email:"",
          orgId:null,
        },
        dialogFormRules: {
          username: [
            {required: true, message: '请输入用户名称', trigger: 'blur'},
          ],
          orgId: [
            {required: true, message: '请选择用户组织', trigger: 'blur'},
          ],
          email: [
            {type: 'email', message: '请输入正确的邮箱地址', trigger: 'blur'}
          ],
          phone: [
            {pattern: /^1[34578]\d{9}$/, message: '目前只支持中国大陆的手机号码', trigger: 'blur'}
          ],
        },
        elTreeDisabled:false,
        elTreeProps:{         // el-tree-select配置项（必选）
          value: 'id',
          label: 'orgName',
          children: 'children',
        },
        handlingUserId:null,
        roleDialogTitle:"",
        roleDialogVisible: false,
        checkedRoleIds:[],
        roleDataProp:{
          key: 'id',
          label: 'roleName'
        },
        roleDatas:[]
      }
    },
    watch: {
      filterText(val) {
        this.$refs.orgQueryTree.filter(val);
      }
    },
    computed:{
      dialogAlertInfo(){
        return "新增用户默认初始密码:" +
          this.$store.getters.getSysConfigItem("user.init.password") +
          ",会在用户登陆后提示用户自行修改。" +
          "如果您希望为用户修改密码，" +
          "请使用密码重置功能"
          
      }
    },
    methods: {
      getData(){
        getUsers(this.userQueryForm,this.pagination)
          .then(res => {
            this.setData(res)
          })
      },
      updateData(){
        updateUser(this.dialogForm)
          .then(res => {
            this.$message({message: res.data, type: 'success'});
            this.submitQueryForm();//修改之后，重新查询table
            this.handleCloseDialog();
          })
      },
      addData(){
        addUser(this.dialogForm).then(res => {
          this.$message({message: res.data, type: 'success'});
          this.submitQueryForm();//新增之后，重新查询table
          this.handleCloseDialog();
        })
      },
      deleteData(row){
        this.$confirm("确定删除["+row.username+"]?")
          .then(_ => {
            deleteUser(row.id)
              .then(res => {
                this.submitQueryForm();//删除之后，重新查询table
                this.$message({message: res.data, type: 'success'});
              }).catch(err => {
              this.$message({message: err.message, type: 'error'});
            })
          });
      },
      changeEnabled(index,row){
        changeEnabled(row.id,row.enabled).then(res => {
          if(res.isok){
            this.$message({message: res.data, type: 'success'});
          }
        })
      },
      handlePageSizeChange(val){
        this.pagination.pageSize = val;
        this.submitQueryForm()
      },
      handlePageNumChange(val){
        this.pagination.pageNum = val;
        this.submitQueryForm()
      },
      filterOrg(value, data) {
        if (!value) return true;
        return data.orgName.indexOf(value) !== -1;
      },
      orgNodeClick(node){
        this.userQueryForm.orgId = node.id
        this.submitQueryForm()
      },
      handleTreeSelected(value){
        this.dialogForm.orgId = value
        this.$refs.dialogForm.validateField("orgId");
      },
      validateSelectTree(){
        this.$refs.dialogForm.validateField("orgId");
      },
      resetPwd(index,row){
        this.$confirm("确定重置密码为："
          + this.$store.getters.getSysConfigItem("user.init.password")
          + "么？")
        .then(_ => {
          resetUserPwd(row.id).then(res => {
            this.$message({message: res.data, type: 'success'})
          });
        });
      },
      assignRole(index,row){
        this.roleDialogVisible = true
        this.roleDialogTitle = "用户" + row.username + "角色分配"
        this.handlingUserId = row.id
        getCheckedRoles(row.id).then(res => {
          this.roleDatas = res.data.roleDatas
          this.checkedRoleIds = res.data.checkedRoleIds
        })
      },
      saveUserRoles(){
        if(this.checkedRoleIds.length > 0){
          saveCheckedUserRoles(this.handlingUserId,this.checkedRoleIds)
            .then(res => {
              this.$message({message: res.data, type: 'success'});
              this.roleDialogVisible = false
            })
        }else{
          this.$message.error("已选角色不能为空");
        }
      },
      setData(pageinfo) {
        if (pageinfo.isok) {
          this.tableData = pageinfo.data.records
          this.pagination.pageSize = pageinfo.data.size
          this.pagination.total = pageinfo.data.total
        }
      },
      setOrgData(orgTree){
        if (orgTree.isok) {
          this.orgData = orgTree.data
        }
      }
    },
    beforeRouteEnter(to, from, next) {
      axios.all([
        getUsers(
          {username:"",phone:"",enabled:"",email:"",orgId:null,timeRange: ["",""]}
          ,{pageNum: 1, pageSize: 20,}
        ),
        getOrgTree({status:false,name:""})])
      .then(axios.spread(function (res1, res2) {
        // 两个请求都执行完成后，进入该函数
        next(vm => {vm.setData(res1);vm.setOrgData(res2)})
      }));
    }
  }
</script>

<style scoped>
  .el-form {
    margin-top: 20px;
  }
  .el-tree--highlight-current /deep/ .el-tree-node.is-current > .el-tree-node__content {
    background-color: rgb(31, 158, 254);
    color: rgb(255, 255, 255);
  }

</style>