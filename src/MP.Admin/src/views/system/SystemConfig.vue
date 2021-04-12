<template>
  <div>
    <el-card body-style="padding: 0">
      <el-form ref="configQueryform" :model="configQueryform" label-width="80px">
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="参数配置" prop="configLike">
              <el-input v-model="configQueryform.configLike"
                        placeholder="请输入参数名称或参数编码"/>
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
                 icon="el-icon-plus" @click="handleAdd('新增全局参数')">新增</el-button>

      <el-table :data="tableData" border default-expand-all stripe style="width: 100%;margin-bottom: 20px;">
        <el-table-column prop="paramName" label="参数中文名称" width="200" align="center"/>
        <el-table-column prop="paramKey" label="参数编码" width="200" align="center"/>
        <el-table-column prop="paramValue" label="参数值" width="200" align="center"/>
        <el-table-column prop="paramDesc" label="参数描述" width="" align="center"/>
        <el-table-column label="操作" width="200" align="center">
          <template slot-scope="scope">
            <el-button size="mini" type="primary" icon="el-icon-edit" circle
                       @click="handleEdit(scope.$index, scope.row,'修改全局参数')"/>
            <el-button size="mini" type="danger" icon="el-icon-delete" circle
                       @click="handleDelete(scope.$index, scope.row)"/>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog :title="dialogTitle" :visible.sync="dialogFormVisible"
               :before-close="beforeDialogClose">
      <el-form :model="dialogForm" ref="dialogForm"
               :rules="dialogFormRules" label-width="20%">

        <el-row :gutter="20">
            <el-form-item label="参数中文名称" prop="paramName">
              <el-input v-model="dialogForm.paramName" autocomplete="off"></el-input>
            </el-form-item>
        </el-row>
        <el-row :gutter="20">
            <el-form-item label="参数编码" prop="paramKey">
              <el-input v-model="dialogForm.paramKey" autocomplete="off"></el-input>
            </el-form-item>
        </el-row>
        <el-row :gutter="20">
            <el-form-item label="参数值" prop="paramValue">
              <el-input v-model="dialogForm.paramValue" autocomplete="off" ></el-input>
            </el-form-item>
        </el-row>
        <el-row :gutter="20">
            <el-form-item label="参数描述" prop="paramDesc">
              <el-input v-model="dialogForm.paramDesc" autocomplete="off" ></el-input>
            </el-form-item>
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
  import {getConfigs,updateConfig,addConfig,deleteConfig}
  from '../../api/system/sys_config'

  import MixinCUD from '@/components/MixinCUD'

  export default {
    name: "SystemConfig",
    mixins:[MixinCUD],
    data() {
      return {
        tableData: [],
        queryFormRefName:"configQueryform",
        configQueryform:{
          configLike: ""
        },
        dialogFormVisible: false,
        dialogTitle:"",
        dialogRefName:"dialogForm",
        dialogForm: {
          id: null,
          paramName: '',
          paramKey: '',
          paramValue: '',
          paramDesc: ''
        },
        dialogFormRules: {
          paramName: [
            {required: true, message: '请输入参数中文名称', trigger: 'blur'},
          ],
          paramKey: [
            {required: true, message: '请输入参数编码', trigger: 'blur'},
            {pattern: /^[A-Za-z0-9.]+$/, message: '只能包含英文、数字和"."', trigger: 'blur'}
          ],
          paramValue: [
            {required: true, message: '请输入参数值', trigger: 'blur'},
          ],
          paramDesc: [
            {required: true, message: '请输入参数描述信息', trigger: 'blur'},
          ]
        }
      }
    },
    methods: {
      handleRefresh(){
        this.$store.dispatch("refreshConfig")
          .then(res => {
            this.$message({message: "刷新配置参数到应用中成功！", type: 'success'});
          })
      },
      getData(){
        getConfigs(this.configQueryform.configLike)
          .then(res => {
            this.setData(res)
          })
      },
      updateData(){
        updateConfig(this.dialogForm)
          .then(res => {
            this.handleRefresh()
            this.$message({message: res.data, type: 'success'});
            this.submitQueryForm();//修改之后，重新查询table
            this.handleCloseDialog();
          })
      },
      addData(){
        addConfig(this.dialogForm).then(res => {
          this.handleRefresh()
          this.$message({message: res.data, type: 'success'});
          this.submitQueryForm();//新增之后，重新查询table
          this.handleCloseDialog();
        })
      },
      deleteData(row){
        this.$confirm("确定删除全局参数:["+row.paramName+"]?")
          .then(_ => {
            deleteConfig(row.id)
              .then(res => {
                this.handleRefresh()
                this.submitQueryForm();//删除之后，重新查询table
                this.$message({message: res.data, type: 'success'});
              }).catch(err => {
                this.$message({message: err.message, type: 'error'});
              })
          });
      },
      setData(configs) {
        if (configs.isok) {
          this.tableData = configs.data
        }
      }
    },
    beforeRouteEnter(to, from, next) {
      getConfigs("").then(res => {
        next(vm => vm.setData(res))
      })
    }
  }
</script>

<style scoped>
  .el-form {
    margin-top: 20px;
    margin-right: 40px;
  }
</style>