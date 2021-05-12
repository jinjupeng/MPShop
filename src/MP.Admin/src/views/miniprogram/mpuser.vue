<template>
  <el-row>
    <el-col :span="24">
      <div>
        <el-card body-style="padding: 0">
          <el-form
            ref="userQueryForm"
            :model="userQueryForm"
            label-width="80px"
          >
            <el-row :gutter="20">
              <el-col :span="6">
                <el-form-item label="用户名称" prop="username">
                  <el-input
                    v-model="userQueryForm.nickname"
                    placeholder="请输入用户名称"
                  />
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
                    end-placeholder="结束日期"
                  >
                  </el-date-picker>
                </el-form-item>
              </el-col>
              <el-col :span="6" :offset="6" padding="0 20px">
                <el-button
                  type="primary"
                  size="small"
                  @click="submitQueryForm()"
                  icon="el-icon-search"
                >
                  查询</el-button
                >
                <el-button
                  type="primary"
                  size="small"
                  plain
                  @click="resetQueryForm('userQueryForm')"
                  icon="el-icon-refresh"
                >
                  重置</el-button
                >
              </el-col>
            </el-row>
          </el-form>
        </el-card>

        <el-card>
          <el-table
            :data="tableData"
            border
            default-expand-all
            stripe
            style="width: 100%; margin-bottom: 20px"
          >
            <el-table-column
              prop="nickName"
              label="用户昵称"
              width="200"
              align="center"
            />
            <el-table-column label="用户头像" width="200" align="center">
              <template slot-scope="scope">
                <img :src="scope.row.avatarUrl" width="40" height="40" />
              </template>
            </el-table-column>
            <el-table-column
              prop="gender"
              label="性别"
              width="120"
              align="center"
            />
            <el-table-column
              prop="country"
              label="国家"
              width="150"
              align="center"
            />
            <el-table-column
              prop="province"
              label="省份"
              width="150"
              align="center"
            />
            <el-table-column
              prop="city"
              label="城市"
              width="150"
              align="center"
            />
            <el-table-column
              prop="createdAt"
              label="创建时间"
              width="200"
              align="center"
            />
            <el-table-column
              prop="updatedAt"
              label="更新时间"
              width="200"
              align="center"
            />
            <el-table-column
              label="操作"
              width="200"
              align="center"
              fixed="right"
            >
              <template slot-scope="scope">
                <el-button
                  size="mini"
                  type="danger"
                  icon="el-icon-delete"
                  circle
                  @click="handleDelete(scope.$index, scope.row)"
                />
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
            style="float: right; margin-bottom: 10px"
          >
          </el-pagination>
        </el-card>
      </div>
    </el-col>
  </el-row>
</template>

<script>
import { getUsers, deleteUser } from "@/api/miniprogram/mp_user";
import axios from "axios";
import MixinCUD from "@/components/MixinCUD";
import ElTreeSelect from "@/components/TreeSelect";
import DictSelect from "@/components/DictSelect";

export default {
  name: "MPUser",
  mixins: [MixinCUD],
  components: { ElTreeSelect, DictSelect },
  data() {
    return {
      tableData: [],
      queryFormRefName: "userQueryForm",
      userQueryForm: {
        nickname: "",
        timeRange: ["", ""],
      },
      pagination: {
        pageNum: 1,
        pageSize: 20,
        total: null,
      },
      dialogFormRules: {
        nickname: [
          { required: true, message: "请输入用户名称", trigger: "blur" },
        ],
      },
    };
  },
  watch: {},
  computed: {},
  methods: {
    getData() {
      getUsers(this.userQueryForm, this.pagination).then((res) => {
        this.setData(res);
      });
    },
    deleteData(row) {
      this.$confirm("确定删除[" + row.nickname + "]?").then((_) => {
        deleteUser(row.id)
          .then((res) => {
            this.submitQueryForm(); //删除之后，重新查询table
            this.$message({ message: res.data, type: "success" });
          })
          .catch((err) => {
            this.$message({ message: err.message, type: "error" });
          });
      });
    },
    handlePageSizeChange(val) {
      this.pagination.pageSize = val;
      this.submitQueryForm();
    },
    handlePageNumChange(val) {
      this.pagination.pageNum = val;
      this.submitQueryForm();
    },
    setData(pageinfo) {
      if (pageinfo.isok) {
        this.tableData = pageinfo.data.records;
        this.pagination.pageSize = pageinfo.data.size;
        this.pagination.total = pageinfo.data.total;
      }
    },
  },
  // 进入该页面之前先请求一次数据
  beforeRouteEnter(to, from, next) {
    axios
      .all([
        getUsers(
          { nickname: "", timeRange: ["", ""] },
          { pageNum: 1, pageSize: 20 }
        ),
      ])
      .then(
        axios.spread(function (res1) {
          // 请求都执行完成后，进入该函数
          next((vm) => {
            vm.setData(res1);
          });
        })
      );
  },
};
</script>

<style scoped>
.el-form {
  margin-top: 20px;
}
.el-tree--highlight-current
  /deep/
  .el-tree-node.is-current
  > .el-tree-node__content {
  background-color: rgb(31, 158, 254);
  color: rgb(255, 255, 255);
}
</style>