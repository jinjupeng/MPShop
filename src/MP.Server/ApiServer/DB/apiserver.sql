/*
 Navicat Premium Data Transfer

 Source Server         : 127.0.0.1_3306
 Source Server Type    : MySQL
 Source Server Version : 80022
 Source Host           : 127.0.0.1:3306
 Source Schema         : apiserver

 Target Server Type    : MySQL
 Target Server Version : 50799
 File Encoding         : 65001

 Date: 08/04/2021 09:26:11
*/

SET NAMES utf8;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for sys_api
-- ----------------------------
DROP TABLE IF EXISTS `sys_api`;
CREATE TABLE `sys_api`  (
  `id` bigint(0) NOT NULL,
  `api_pid` bigint(0) NOT NULL COMMENT '接口父ID(即接口分组)',
  `api_pids` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '当前接口的所有上级id(即所有上级分组)',
  `is_leaf` tinyint(1) NOT NULL COMMENT '0:不是叶子节点，1:是叶子节点',
  `api_name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '接口名称',
  `url` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '跳转URL',
  `sort` int(0) NULL DEFAULT NULL COMMENT '排序',
  `level` int(0) NOT NULL COMMENT '层级，1：接口分组，2：接口',
  `status` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否禁用，0:启用(否）,1:禁用(是)',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '系统Http接口表，配合sys_role_api控制接口访问权限';

-- ----------------------------
-- Records of sys_api
-- ----------------------------
BEGIN;
INSERT INTO `sys_api` VALUES (1, 0, '[0]', 0, '系统数据接口', NULL, 1, 1, 0), (2, 1, '[0],[1]', 0, '系统管理模块', NULL, 1, 2, 0), (3, 2, '[0],[1],[2]', 1, '用户信息接口', '/sysuser/info', 1, 3, 0), (4, 2, '[0],[1],[2]', 1, '组织管理-树形数据接口', '/sysorg/tree', 2, 3, 0), (5, 2, '[0],[1],[2]', 1, '组织管理-新增组织接口', '/sysorg/add', 3, 3, 0), (6, 2, '[0],[1],[2]', 1, '组织管理-修改组织接口', '/sysorg/update', 4, 3, 0), (7, 2, '[0],[1],[2]', 1, '组织管理-删除组织接口', '/sysorg/delete', 5, 3, 0), (8, 2, '[0],[1],[2]', 1, '菜单树形数据加载接口', '/sysmenu/tree', 6, 3, 0), (9, 2, '[0],[1],[2]', 1, '菜单管理-新增菜单项接口', '/sysmenu/add', 7, 3, 0), (10, 2, '[0],[1],[2]', 1, '菜单管理-修改菜单项接口', '/sysmenu/update', 8, 3, 0), (11, 2, '[0],[1],[2]', 1, '菜单管理-删除菜单项接口', '/sysmenu/delete', 9, 3, 0), (12, 2, '[0],[1],[2]', 1, '查询某角色已具备菜单权限接口', '/sysmenu/checkedtree', 10, 3, 0), (13, 2, '[0],[1],[2]', 1, '保存某角色分配勾选的菜单权限', '/sysmenu/savekeys', 11, 3, 0), (14, 2, '[0],[1],[2]', 1, '接口分类树形结构数据加载', '/sysapi/tree', 12, 3, 0), (15, 2, '[0],[1],[2]', 1, '接口管理-新增接口', '/sysapi/add', 13, 3, 0), (16, 2, '[0],[1],[2]', 1, '接口管理-更新接口数据', '/sysapi/update', 14, 3, 0), (17, 2, '[0],[1],[2]', 1, '接口管理-删除接口', '/sysapi/delete', 15, 3, 0), (18, 2, '[0],[1],[2]', 1, '查询某角色已具备的接口访问权限', '/sysapi/checkedtree', 16, 3, 0), (19, 2, '[0],[1],[2]', 1, '保存某角色勾选分配的接口访问权限', '/sysapi/savekeys', 17, 3, 0), (20, 2, '[0],[1],[2]', 1, '角色管理-列表查询', '/sysrole/query', 18, 3, 0), (21, 2, '[0],[1],[2]', 1, '角色管理-新增角色', '/sysrole/add', 19, 3, 0), (22, 2, '[0],[1],[2]', 1, '角色管理-更新角色数据', '/sysrole/update', 20, 3, 0), (23, 2, '[0],[1],[2]', 1, '角色管理-删除角色', '/sysrole/delete', 21, 3, 0), (24, 2, '[0],[1],[2]', 1, '查询某用户具备的角色id列表', '/sysrole/checkedroles', 22, 3, 0), (25, 2, '[0],[1],[2]', 1, '保存为某用户分配的角色', '/sysrole/savekeys', 23, 3, 0), (26, 2, '[0],[1],[2]', 1, '用户管理-用户列表查询', '/sysuser/query', 24, 3, 0), (27, 2, '[0],[1],[2]', 1, '用户管理-新增用户', '/sysuser/add', 25, 3, 0), (28, 2, '[0],[1],[2]', 1, '用户管理-修改用户信息', '/sysuser/update', 26, 3, 0), (29, 2, '[0],[1],[2]', 1, '用户管理-删除用户', '/sysuser/delete', 27, 3, 0), (30, 2, '[0],[1],[2]', 1, '为用户重置密码', '/sysuser/pwd/reset', 28, 3, 0), (31, 2, '[0],[1],[2]', 1, '判断用户是否使用默认密码', '/sysuser/pwd/isdefault', 29, 3, 0), (32, 2, '[0],[1],[2]', 1, '修改用户密码', '/sysuser/pwd/change', 30, 3, 0), (33, 2, '[0],[1],[2]', 1, '菜单栏数据接口(根据登录用户)', '/sysmenu/tree/user', 6, 3, 0), (34, 2, '[0],[1],[2]', 1, '获取系统全局配置参数', '/sysconfig/all', 31, 3, 0), (35, 2, '[0],[1],[2]', 1, '条件查询全局配置参数接口', '/sysconfig/query', 32, 3, 0), (36, 2, '[0],[1],[2]', 1, '新增配置参数接口', '/sysconfig/add', 33, 3, 0), (37, 2, '[0],[1],[2]', 1, '修改配置参数接口', '/sysconfig/update', 34, 3, 0), (38, 2, '[0],[1],[2]', 1, '删除配置参数接口', '/sysconfig/delete', 35, 3, 0), (39, 2, '[0],[1],[2]', 1, '配置参数从数据库刷新到内存', '/sysconfig/refresh', 36, 3, 0), (40, 2, '[0],[1],[2]', 1, '数据字典数据加载接口', '/sysdict/all', 37, 3, 0), (41, 2, '[0],[1],[2]', 1, '数据字典条件查询接口', '/sysdict/query', 38, 3, 0), (42, 2, '[0],[1],[2]', 1, '数据字典数据新增接口', '/sysdict/add', 39, 3, 0), (43, 2, '[0],[1],[2]', 1, '数据字典数据修改接口', '/sysdict/update', 40, 3, 0), (44, 2, '[0],[1],[2]', 1, '数据字典数据删除接口', '/sysdict/delete', 41, 3, 0);
COMMIT;

-- ----------------------------
-- Table structure for sys_config
-- ----------------------------
DROP TABLE IF EXISTS `sys_config`;
CREATE TABLE `sys_config`  (
  `id` bigint(0) NOT NULL,
  `param_name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '参数名称(中文)',
  `param_key` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '参数编码唯一标识(英文及数字)',
  `param_value` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '参数值',
  `param_desc` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '参数描述备注',
  `create_time` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0) ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `param_key`(`param_key`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '系统全局配置参数';

-- ----------------------------
-- Records of sys_config
-- ----------------------------
BEGIN;
INSERT INTO `sys_config` VALUES (1, '用户初始密码', 'user.init.password', 'abcd1234', '系统新增用户初始化密码（登录后会提示用户自行修改）', '2020-02-29 13:26:58');
COMMIT;

-- ----------------------------
-- Table structure for sys_dict
-- ----------------------------
DROP TABLE IF EXISTS `sys_dict`;
CREATE TABLE `sys_dict`  (
  `id` bigint(0) NOT NULL,
  `group_name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '分组名称',
  `group_code` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '分组编码',
  `item_name` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '字典项名称',
  `item_value` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '字典项Value',
  `item_desc` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '字典项描述',
  `create_time` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0) ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '字典项创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '数据字典表';

-- ----------------------------
-- Records of sys_dict
-- ----------------------------
BEGIN;
INSERT INTO `sys_dict` VALUES (1, '是否禁用', 'common.status', '未禁用', 'false', '通用数据记录的禁用状态', '2020-02-29 17:00:16'), (2, '是否禁用', 'common.status', '已禁用', 'true', '通用数据记录的禁用状态', '2020-02-29 17:00:26'), (3, '用户状态', 'sysuser.enabled', '已激活', 'true', '用户状态', '2020-02-29 18:42:08'), (4, '用户状态', 'sysuser.enabled', '已禁用', 'false', '用户状态', '2020-02-29 23:23:35');
COMMIT;

-- ----------------------------
-- Table structure for sys_menu
-- ----------------------------
DROP TABLE IF EXISTS `sys_menu`;
CREATE TABLE `sys_menu`  (
  `id` bigint(0) NOT NULL,
  `menu_pid` bigint(0) NOT NULL COMMENT '父菜单ID',
  `menu_pids` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '当前菜单所有父菜单',
  `is_leaf` tinyint(1) NOT NULL COMMENT '0:不是叶子节点，1:是叶子节点',
  `menu_name` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '菜单名称',
  `url` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '跳转URL',
  `icon` varchar(45) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `sort` int(0) NULL DEFAULT NULL COMMENT '排序',
  `level` int(0) NOT NULL COMMENT '菜单层级',
  `status` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否禁用，0:启用(否）,1:禁用(是)',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '系统菜单表';

-- ----------------------------
-- Records of sys_menu
-- ----------------------------
BEGIN;
INSERT INTO `sys_menu` VALUES (1, 0, '[0]', 0, '系统根目录', '/', '', 1, 1, 0), (2, 1, '[0],[1]', 0, '系统管理', '/system', 'el-icon-fa fa-cogs', 1, 2, 0), (3, 2, '[0],[1],[2]', 1, '用户管理', '/home/sysuser', 'el-icon-fa fa-user', 1, 3, 0), (4, 2, '[0],[1],[2]', 1, '角色管理', '/home/sysrole', 'el-icon-fa fa-users', 2, 3, 0), (5, 2, '[0],[1],[2]', 1, '组织管理', '/home/sysorg', 'el-icon-fa fa-sitemap', 3, 3, 0), (6, 2, '[0],[1],[2]', 1, '菜单管理', '/home/sysmenu', 'el-icon-fa fa-list-ul', 4, 3, 0), (7, 2, '[0],[1],[2]', 1, '接口管理', '/home/sysapi', 'el-icon-fa fa-plug', 5, 3, 0), (10, 1, '[0],[1]', 0, '测试用菜单', '/order', 'el-icon-eleme', 2, 2, 0), (11, 10, '[0],[1],[10]', 1, '子菜单(首页)', '/home/firstpage', 'el-icon-lock', 1, 3, 0), (12, 2, '[0],[1],[2]', 1, '参数配置', '/home/sysconfig', 'el-icon-fa fa-cog', 6, 3, 0), (13, 2, '[0],[1],[2]', 1, '数据字典', '/home/sysdict', 'el-icon-fa fa-list-ol', 7, 3, 0);
COMMIT;

-- ----------------------------
-- Table structure for sys_org
-- ----------------------------
DROP TABLE IF EXISTS `sys_org`;
CREATE TABLE `sys_org`  (
  `id` bigint(0) NOT NULL,
  `org_pid` bigint(0) NOT NULL COMMENT '上级组织编码',
  `org_pids` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '所有的父节点id',
  `is_leaf` tinyint(1) NOT NULL COMMENT '0:不是叶子节点，1:是叶子节点',
  `org_name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '组织名',
  `address` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '地址',
  `phone` varchar(13) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '电话',
  `email` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '邮件',
  `sort` int(0) NULL DEFAULT NULL COMMENT '排序',
  `level` int(0) NOT NULL COMMENT '组织层级',
  `status` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否禁用，0:启用(否）,1:禁用(是)',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '系统组织结构表';

-- ----------------------------
-- Records of sys_org
-- ----------------------------
BEGIN;
INSERT INTO `sys_org` VALUES (1, 0, '[0]', 0, 'DongTech', NULL, NULL, NULL, 1, 1, 0), (1298067029349117953, 1, '[0],[1]', 0, '西安分公司', NULL, '13215678932', '11111111@qq.com', 1, 2, 0), (1298067159791972353, 1298067029349117953, '[0],[1],[1298067029349117953]', 1, '测试部一', NULL, '', '11111111@qq.com', 1, 3, 0), (1298067674592456706, 1, '[0],[1]', 0, '上海分公司', NULL, '', '11111111@qq.com', 2, 2, 0), (1298067729978241025, 1298067674592456706, '[0],[1],[1298067674592456706]', 1, '运维部一', NULL, '', '11111111@qq.com', 1, 3, 0), (1298067787712835585, 1298067674592456706, '[0],[1],[1298067674592456706]', 1, '运维部二', NULL, '', '11111111@qq.com', 2, 3, 0), (1298067843731959809, 1298067674592456706, '[0],[1],[1298067674592456706]', 1, '运维部三', NULL, '', '11111111@qq.com', 3, 3, 0), (1298068119314509825, 1298067029349117953, '[0],[1],[1298067029349117953]', 1, '研发部一', NULL, '', '11111111@qq.com', 2, 3, 0);
COMMIT;

-- ----------------------------
-- Table structure for sys_role
-- ----------------------------
DROP TABLE IF EXISTS `sys_role`;
CREATE TABLE `sys_role`  (
  `id` bigint(0) NOT NULL,
  `role_name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' COMMENT '角色名称(汉字)',
  `role_desc` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' COMMENT '角色描述',
  `role_code` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' COMMENT '角色的英文code.如：ADMIN',
  `sort` int(0) NOT NULL DEFAULT 0 COMMENT '角色顺序',
  `status` tinyint(1) NULL DEFAULT 0 COMMENT '是否禁用，0:启用(否）,1:禁用(是)',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '系统角色表';

-- ----------------------------
-- Records of sys_role
-- ----------------------------
BEGIN;
INSERT INTO `sys_role` VALUES (-108423392314523648, 'roleName', 'roleDesc', 'roleCode', 1, 0), (1298061556168273921, '管理员', '系统管理员', 'admin', 1, 0), (1298063367197437954, '普通用户', '普通用户', 'common', 2, 0);
COMMIT;

-- ----------------------------
-- Table structure for sys_role_api
-- ----------------------------
DROP TABLE IF EXISTS `sys_role_api`;
CREATE TABLE `sys_role_api`  (
  `role_id` bigint(0) NOT NULL COMMENT '角色id',
  `api_id` bigint(0) NOT NULL COMMENT '接口id'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '角色接口权限关系表';

-- ----------------------------
-- Records of sys_role_api
-- ----------------------------
BEGIN;
INSERT INTO `sys_role_api` VALUES (1298061556168273921, 1), (1298061556168273921, 2), (1298061556168273921, 3), (1298061556168273921, 4), (1298061556168273921, 5), (1298061556168273921, 6), (1298061556168273921, 7), (1298061556168273921, 33), (1298061556168273921, 8), (1298061556168273921, 9), (1298061556168273921, 10), (1298061556168273921, 11), (1298061556168273921, 12), (1298061556168273921, 13), (1298061556168273921, 14), (1298061556168273921, 15), (1298061556168273921, 16), (1298061556168273921, 17), (1298061556168273921, 18), (1298061556168273921, 19), (1298061556168273921, 20), (1298061556168273921, 21), (1298061556168273921, 22), (1298061556168273921, 23), (1298061556168273921, 24), (1298061556168273921, 25), (1298061556168273921, 26), (1298061556168273921, 27), (1298061556168273921, 28), (1298061556168273921, 29), (1298061556168273921, 30), (1298061556168273921, 31), (1298061556168273921, 32), (1298061556168273921, 34), (1298061556168273921, 35), (1298061556168273921, 36), (1298061556168273921, 37), (1298061556168273921, 38), (1298061556168273921, 39), (1298061556168273921, 40), (1298061556168273921, 41), (1298061556168273921, 42), (1298061556168273921, 43), (1298061556168273921, 44), (1298063367197437954, 1), (1298063367197437954, 2), (1298063367197437954, 3), (1298063367197437954, 4), (1298063367197437954, 5), (1298063367197437954, 6), (1298063367197437954, 7), (1298063367197437954, 33), (1298063367197437954, 8), (1298063367197437954, 9), (1298063367197437954, 10), (1298063367197437954, 11), (1298063367197437954, 12), (1298063367197437954, 13), (1298063367197437954, 14), (1298063367197437954, 15), (1298063367197437954, 16), (1298063367197437954, 17), (1298063367197437954, 18), (1298063367197437954, 19), (1298063367197437954, 20), (1298063367197437954, 21), (1298063367197437954, 22), (1298063367197437954, 23), (1298063367197437954, 24), (1298063367197437954, 25), (1298063367197437954, 26), (1298063367197437954, 27), (1298063367197437954, 28), (1298063367197437954, 29), (1298063367197437954, 30), (1298063367197437954, 31), (1298063367197437954, 32), (1298063367197437954, 34), (1298063367197437954, 35), (1298063367197437954, 36), (1298063367197437954, 37), (1298063367197437954, 38), (1298063367197437954, 39), (1298063367197437954, 40), (1298063367197437954, 41), (1298063367197437954, 42), (1298063367197437954, 43), (1298063367197437954, 44);
COMMIT;

-- ----------------------------
-- Table structure for sys_role_menu
-- ----------------------------
DROP TABLE IF EXISTS `sys_role_menu`;
CREATE TABLE `sys_role_menu`  (
  `role_id` bigint(0) NOT NULL DEFAULT 0 COMMENT '角色id',
  `menu_id` bigint(0) NOT NULL DEFAULT 0 COMMENT '权限id'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '角色菜单权限关系表';

-- ----------------------------
-- Records of sys_role_menu
-- ----------------------------
BEGIN;
INSERT INTO `sys_role_menu` VALUES (1298063367197437954, 1), (1298063367197437954, 2), (1298063367197437954, 3), (1298063367197437954, 4), (1298063367197437954, 5), (1298063367197437954, 6), (1298063367197437954, 7), (1298063367197437954, 12), (1298063367197437954, 13), (1298063367197437954, 10), (1298063367197437954, 11), (1298061556168273921, 1), (1298061556168273921, 2), (1298061556168273921, 3), (1298061556168273921, 4), (1298061556168273921, 5), (1298061556168273921, 6), (1298061556168273921, 7), (1298061556168273921, 12), (1298061556168273921, 13), (1298061556168273921, 10), (1298061556168273921, 11);
COMMIT;

-- ----------------------------
-- Table structure for sys_user
-- ----------------------------
DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE `sys_user`  (
  `id` bigint(0) NOT NULL,
  `username` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' COMMENT '用户名',
  `password` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' COMMENT '密码',
  `nickname` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '昵称',
  `portrait` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '头像图片路径',
  `org_id` bigint(0) NOT NULL COMMENT '组织id',
  `enabled` tinyint(1) NOT NULL DEFAULT 1 COMMENT '0无效用户，1是有效用户',
  `phone` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '手机号',
  `email` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'email',
  `create_time` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0) ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '用户创建时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `username`(`username`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '用户信息表';

-- ----------------------------
-- Records of sys_user
-- ----------------------------
BEGIN;
INSERT INTO `sys_user` VALUES (1297873308628307970, 'admin', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', NULL, NULL, 1, 1, '13214456783', 'hahaha1@163.com', '2020-12-26 11:10:13');
COMMIT;

-- ----------------------------
-- Table structure for sys_user_role
-- ----------------------------
DROP TABLE IF EXISTS `sys_user_role`;
CREATE TABLE `sys_user_role`  (
  `role_id` bigint(0) NOT NULL DEFAULT 0 COMMENT '角色自增id',
  `user_id` bigint(0) NOT NULL DEFAULT 0 COMMENT '用户自增id'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '用户角色关系表';

-- ----------------------------
-- Records of sys_user_role
-- ----------------------------
BEGIN;
INSERT INTO `sys_user_role` VALUES (1298061556168273921, 1297873308628307970);
COMMIT;

SET FOREIGN_KEY_CHECKS = 1;
