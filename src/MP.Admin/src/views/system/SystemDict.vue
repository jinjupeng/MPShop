<template>
  <div>
    <el-card body-style="padding: 0">
      <el-form ref="queryForm" :model="queryForm" label-width="80px">
        <el-row :gutter="20">
          <el-col :span="6">
            <el-form-item label="分组名称" prop="groupName">
              <el-input v-model="queryForm.groupName"
                        placeholder="请输入数据字典分组名称"/>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="分组编码" prop="groupCode">
              <el-input v-model="queryForm.groupCode"
                        placeholder="请输入数据字典分组编码"/>
            </el-form-item>
          </el-col>
          <el-col :span="6" :offset="6">
            <el-form-item>
              <el-button type="primary" size="small"
                         @click="submitQueryForm()" icon="el-icon-search">
                查询</el-button>
              <el-button type="primary" size="small" plain
                         @click="resetQueryForm()" icon="el-icon-refresh">
                重置</el-button>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>

    <el-card>
      <el-button type="primary" size="small" style="margin: 0 0 10px 20px"
                 icon="el-icon-plus" @click="handleAdd('新增字典项')">新增</el-button>


      <el-table :data="tableData" border default-expand-all stripe style="width: 100%;margin-bottom: 20px;">
        <el-table-column prop="groupName" label="分组名称" width="200" align="center" />
        <el-table-column prop="groupCode" label="分组编码" width="200" align="center"/>
        <el-table-column prop="itemName" label="字典项名称" width="200" align="center"/>
        <el-table-column prop="itemValue" label="字典项Value" width="200" align="center"/>
        <el-table-column prop="itemDesc" label="字典项描述" width="" align="center"/>
        <el-table-column label="操作" width="200" align="center">
          <template slot-scope="scope">
            <el-button size="mini" type="primary" icon="el-icon-edit" circle
                       @click="handleEdit(scope.$index, scope.row,'修改字典项')"/>
            <el-button size="mini" type="danger" icon="el-icon-delete" circle
                       @click="handleDelete(scope.$index, scope.row)"/>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog :title="dialogTitle" :visible.sync="dialogFormVisible"
               :before-close="beforeDialogClose">
      <el-form :model="dialogForm" ref="dialogForm"
               :rules="dialogFormRules" label-width="100px">

            <el-form-item label="分组名称" prop="groupName">
              <el-input v-model="dialogForm.groupName" autocomplete="off"></el-input>
            </el-form-item>
            <el-form-item label="分组编码" prop="groupCode">
              <el-input v-model="dialogForm.groupCode" autocomplete="off"></el-input>
            </el-form-item>
            <el-form-item label="字典项名称" prop="itemName">
              <el-input v-model="dialogForm.itemName" autocomplete="off" ></el-input>
            </el-form-item>
            <el-form-item label="字典项Value" prop="itemValue">
              <el-input v-model="dialogForm.itemValue" autocomplete="off" ></el-input>
            </el-form-item>
            <el-form-item label="字典项描述" prop="itemDesc">
              <el-input v-model="dialogForm.itemDesc" autocomplete="off" ></el-input>
            </el-form-item>

      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="submitDialogForm" size="mini" type="primary">确 定</el-button>
        <el-button @click="handleCloseDialog" size="mini">取 消</el-button>
      </div>
    </el-dialog>

  </div>
</template>

<script>
  import {getAllSysDict,getSysDict,updateSysDict,addSysDict,deleteSysDict} from '../../api/system/sys_dict'

  import MixinCUD from '@/components/MixinCUD'

  export default {
    name: "SystemDict",
    mixins:[MixinCUD],
    data() {
      return {
        tableData: [],
        queryFormRefName:"queryForm",
        queryForm:{
          groupName: "",
          groupCode: ""
        },
        dialogFormVisible: false,
        dialogTitle:"",
        dialogRefName:"dialogForm",
        dialogForm: {
          id: null,
          groupName: '',
          groupCode: '',
          itemName: '',
          itemValue:'',
          itemDesc: ''
        },
        dialogFormRules: {
          groupName: [
            {required: true, message: '请输入分组名称', trigger: 'blur'},
          ],
          groupCode: [
            {required: true, message: '请输入分组编码', trigger: 'blur'},
          ],
          itemName: [
            {required: true, message: '请输入字典项名称', trigger: 'blur'},
          ],
          itemValue: [
            {required: true, message: '请输入字典项Value', trigger: 'blur'},
          ]
        }
      }
    },
    methods: {
      handleRefresh(){
        this.$store.dispatch("refreshDict")
          .then(res => {
            this.$message({message: "刷新配置参数到应用中成功！", type: 'success'});
          })
      },
      getData(){
        getSysDict(this.queryForm.groupName,this.queryForm.groupCode)
          .then(res => {
            this.setData(res)
          })
      },
      updateData(){
        updateSysDict(this.dialogForm)
          .then(res => {
            this.handleRefresh()
            this.$message({message: res.data, type: 'success'});
            this.submitQueryForm();//修改之后，重新查询table
            this.handleCloseDialog();
          })
      },
      addData(){
        addSysDict(this.dialogForm).then(res => {
          this.handleRefresh()
          this.$message({message: res.data, type: 'success'});
          this.submitQueryForm();//新增之后，重新查询table
          this.handleCloseDialog();
        })
      },
      deleteData(row){
        this.$confirm("确定删除数据字典:["+row.groupName+":"+row.itemName+"]?")
          .then(_ => {
            deleteSysDict(row.id)
              .then(res => {
                this.handleRefresh()
                this.submitQueryForm();//删除之后，重新查询table
                this.$message({message: res.data, type: 'success'});
              }).catch(err => {
                this.$message({message: err.message, type: 'error'});
              })
          });
      },
      setData(res) {
        if (res.isok) {
          this.tableData = res.data
        }
      }
    },
    beforeRouteEnter(to, from, next) {
      getAllSysDict().then(res => {
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