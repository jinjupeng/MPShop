using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class ContextMySql : DbContext
    {
        public ContextMySql()
        {
        }

        public ContextMySql(DbContextOptions<ContextMySql> options)
            : base(options)
        {
        }

        public virtual DbSet<address> address { get; set; }
        public virtual DbSet<brand> brand { get; set; }
        public virtual DbSet<goods> goods { get; set; }
        public virtual DbSet<goods_attr_key> goods_attr_key { get; set; }
        public virtual DbSet<goods_attr_value> goods_attr_value { get; set; }
        public virtual DbSet<goods_carts> goods_carts { get; set; }
        public virtual DbSet<goods_category> goods_category { get; set; }
        public virtual DbSet<goods_info> goods_info { get; set; }
        public virtual DbSet<goods_sku> goods_sku { get; set; }
        public virtual DbSet<mp_user> mp_user { get; set; }
        public virtual DbSet<order> order { get; set; }
        public virtual DbSet<session_key> session_key { get; set; }
        public virtual DbSet<sys_api> sys_api { get; set; }
        public virtual DbSet<sys_config> sys_config { get; set; }
        public virtual DbSet<sys_dict> sys_dict { get; set; }
        public virtual DbSet<sys_menu> sys_menu { get; set; }
        public virtual DbSet<sys_org> sys_org { get; set; }
        public virtual DbSet<sys_role> sys_role { get; set; }
        public virtual DbSet<sys_role_api> sys_role_api { get; set; }
        public virtual DbSet<sys_role_menu> sys_role_menu { get; set; }
        public virtual DbSet<sys_user> sys_user { get; set; }
        public virtual DbSet<sys_user_role> sys_user_role { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=127.0.0.1;port=3306;database=mpshop;user id=root;password=123456;allow user variables=True", x => x.ServerVersion("8.0.13-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<address>(entity =>
            {
                entity.HasIndex(e => e.tel_number)
                    .HasName("address_tel_number")
                    .IsUnique();

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.created_at).HasColumnType("datetime");

                entity.Property(e => e.detail_info)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.region)
                    .IsRequired()
                    .HasColumnType("json");

                entity.Property(e => e.tel_number)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.updated_at).HasColumnType("datetime");

                entity.Property(e => e.user_id).HasColumnType("int(11)");

                entity.Property(e => e.user_name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<brand>(entity =>
            {
                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.brand_name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<goods>(entity =>
            {
                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.brand_id).HasColumnType("bigint(11)");

                entity.Property(e => e.category_id).HasColumnType("bigint(11)");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.goods_desc)
                    .IsRequired()
                    .HasColumnType("tinytext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.goods_name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.spu_no)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.start_price).HasColumnType("decimal(9,2)");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<goods_attr_key>(entity =>
            {
                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.attr_key)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.goods_id).HasColumnType("int(20)");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<goods_attr_value>(entity =>
            {
                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.attr_key_id).HasColumnType("int(20)");

                entity.Property(e => e.attr_value)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.goods_id).HasColumnType("int(20)");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<goods_carts>(entity =>
            {
                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.goods_id).HasColumnType("int(20)");

                entity.Property(e => e.goods_sku_desc)
                    .IsRequired()
                    .HasColumnType("tinytext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.goods_sku_id).HasColumnType("int(20)");

                entity.Property(e => e.num).HasColumnType("int(4)");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");

                entity.Property(e => e.user_id).HasColumnType("int(20)");
            });

            modelBuilder.Entity<goods_category>(entity =>
            {
                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.category_name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<goods_info>(entity =>
            {
                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.content)
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.goods_id).HasColumnType("int(20)");

                entity.Property(e => e.kind).HasColumnType("int(4)");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<goods_sku>(entity =>
            {
                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.goods_attr_path)
                    .IsRequired()
                    .HasColumnType("json");

                entity.Property(e => e.goods_id).HasColumnType("int(20)");

                entity.Property(e => e.goods_sku_desc)
                    .IsRequired()
                    .HasColumnType("tinytext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.price).HasColumnType("int(11)");

                entity.Property(e => e.stock).HasColumnType("int(4)");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<mp_user>(entity =>
            {
                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.avatarUrl)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.city)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.country)
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.gender).HasColumnType("int(11)");

                entity.Property(e => e.language)
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.nickName)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.openId)
                    .IsRequired()
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.province)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<order>(entity =>
            {
                entity.HasIndex(e => e.out_trade_no)
                    .HasName("order_out_trade_no")
                    .IsUnique();

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.address_desc)
                    .IsRequired()
                    .HasColumnType("tinytext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.address_id).HasColumnType("int(20)");

                entity.Property(e => e.created_at).HasColumnType("datetime");

                entity.Property(e => e.goods_carts_ids)
                    .IsRequired()
                    .HasColumnType("json");

                entity.Property(e => e.goods_name_desc)
                    .IsRequired()
                    .HasColumnType("tinytext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.out_trade_no)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.pay_state).HasColumnType("int(11)");

                entity.Property(e => e.total_fee).HasColumnType("int(11)");

                entity.Property(e => e.transaction_id)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.updated_at).HasColumnType("datetime");

                entity.Property(e => e.user_id).HasColumnType("int(20)");
            });

            modelBuilder.Entity<session_key>(entity =>
            {
                entity.HasIndex(e => e.uid)
                    .HasName("uid");

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.createdAt).HasColumnType("datetime");

                entity.Property(e => e.sessionKey)
                    .IsRequired()
                    .HasColumnType("varchar(24)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.uid).HasColumnType("int(11)");

                entity.Property(e => e.updatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.u)
                    .WithMany(p => p.session_key)
                    .HasForeignKey(d => d.uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("session_key_ibfk_1");
            });

            modelBuilder.Entity<sys_api>(entity =>
            {
                entity.HasComment("系统Http接口表，配合sys_role_api控制接口访问权限");

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.api_name)
                    .IsRequired()
                    .HasColumnType("varchar(64)")
                    .HasComment("接口名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.api_pid)
                    .HasColumnType("int(11)")
                    .HasComment("接口父ID(即接口分组)");

                entity.Property(e => e.api_pids)
                    .IsRequired()
                    .HasColumnType("varchar(128)")
                    .HasComment("当前接口的所有上级id(即所有上级分组)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.is_leaf).HasComment("0:不是叶子节点，1:是叶子节点");

                entity.Property(e => e.level)
                    .HasColumnType("int(11)")
                    .HasComment("层级，1：接口分组，2：接口");

                entity.Property(e => e.sort)
                    .HasColumnType("int(11)")
                    .HasComment("排序");

                entity.Property(e => e.status).HasComment("是否禁用，0:启用(否）,1:禁用(是)");

                entity.Property(e => e.url)
                    .HasColumnType("varchar(64)")
                    .HasComment("跳转URL")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<sys_config>(entity =>
            {
                entity.HasComment("系统全局配置参数");

                entity.HasIndex(e => e.param_key)
                    .HasName("param_key")
                    .IsUnique();

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.create_time)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("创建时间")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.param_desc)
                    .HasColumnType("varchar(64)")
                    .HasComment("参数描述备注")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.param_key)
                    .IsRequired()
                    .HasColumnType("varchar(64)")
                    .HasComment("参数编码唯一标识(英文及数字)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.param_name)
                    .IsRequired()
                    .HasColumnType("varchar(32)")
                    .HasComment("参数名称(中文)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.param_value)
                    .IsRequired()
                    .HasColumnType("varchar(64)")
                    .HasComment("参数值")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<sys_dict>(entity =>
            {
                entity.HasComment("数据字典表");

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.create_time)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("字典项创建时间")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.group_code)
                    .IsRequired()
                    .HasColumnType("varchar(64)")
                    .HasComment("分组编码")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.group_name)
                    .IsRequired()
                    .HasColumnType("varchar(64)")
                    .HasComment("分组名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.item_desc)
                    .HasColumnType("varchar(64)")
                    .HasComment("字典项描述")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.item_name)
                    .IsRequired()
                    .HasColumnType("varchar(16)")
                    .HasComment("字典项名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.item_value)
                    .IsRequired()
                    .HasColumnType("varchar(16)")
                    .HasComment("字典项Value")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<sys_menu>(entity =>
            {
                entity.HasComment("系统菜单表");

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.icon)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.is_leaf).HasComment("0:不是叶子节点，1:是叶子节点");

                entity.Property(e => e.level)
                    .HasColumnType("int(11)")
                    .HasComment("菜单层级");

                entity.Property(e => e.menu_name)
                    .IsRequired()
                    .HasColumnType("varchar(16)")
                    .HasComment("菜单名称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.menu_pid)
                    .HasColumnType("int(11)")
                    .HasComment("父菜单ID");

                entity.Property(e => e.menu_pids)
                    .IsRequired()
                    .HasColumnType("varchar(128)")
                    .HasComment("当前菜单所有父菜单")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.sort)
                    .HasColumnType("int(11)")
                    .HasComment("排序");

                entity.Property(e => e.status).HasComment("是否禁用，0:启用(否）,1:禁用(是)");

                entity.Property(e => e.url)
                    .HasColumnType("varchar(64)")
                    .HasComment("跳转URL")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<sys_org>(entity =>
            {
                entity.HasComment("系统组织结构表");

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.address)
                    .HasColumnType("varchar(64)")
                    .HasComment("地址")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.email)
                    .HasColumnType("varchar(32)")
                    .HasComment("邮件")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.is_leaf).HasComment("0:不是叶子节点，1:是叶子节点");

                entity.Property(e => e.level)
                    .HasColumnType("int(11)")
                    .HasComment("组织层级");

                entity.Property(e => e.org_name)
                    .IsRequired()
                    .HasColumnType("varchar(32)")
                    .HasComment("组织名")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.org_pid)
                    .HasColumnType("int(11)")
                    .HasComment("上级组织编码");

                entity.Property(e => e.org_pids)
                    .IsRequired()
                    .HasColumnType("varchar(128)")
                    .HasComment("所有的父节点id")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.phone)
                    .HasColumnType("varchar(13)")
                    .HasComment("电话")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.sort)
                    .HasColumnType("int(11)")
                    .HasComment("排序");

                entity.Property(e => e.status).HasComment("是否禁用，0:启用(否）,1:禁用(是)");
            });

            modelBuilder.Entity<sys_role>(entity =>
            {
                entity.HasComment("系统角色表");

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.role_code)
                    .IsRequired()
                    .HasColumnType("varchar(32)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("角色的英文code.如：ADMIN")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.role_desc)
                    .IsRequired()
                    .HasColumnType("varchar(128)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("角色描述")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.role_name)
                    .IsRequired()
                    .HasColumnType("varchar(32)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("角色名称(汉字)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.sort)
                    .HasColumnType("int(11)")
                    .HasComment("角色顺序");

                entity.Property(e => e.status)
                    .HasDefaultValueSql("'0'")
                    .HasComment("是否禁用，0:启用(否）,1:禁用(是)");
            });

            modelBuilder.Entity<sys_role_api>(entity =>
            {
                entity.HasComment("角色接口权限关系表");

                entity.Property(e => e.id).HasColumnType("bigint(20)");

                entity.Property(e => e.api_id)
                    .HasColumnType("bigint(20)")
                    .HasComment("接口id");

                entity.Property(e => e.role_id)
                    .HasColumnType("bigint(20)")
                    .HasComment("角色id");
            });

            modelBuilder.Entity<sys_role_menu>(entity =>
            {
                entity.HasComment("角色菜单权限关系表");

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.menu_id)
                    .HasColumnType("int(11)")
                    .HasComment("权限id");

                entity.Property(e => e.role_id)
                    .HasColumnType("int(11)")
                    .HasComment("角色id");
            });

            modelBuilder.Entity<sys_user>(entity =>
            {
                entity.HasComment("用户信息表");

                entity.HasIndex(e => e.username)
                    .HasName("username")
                    .IsUnique();

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.create_time)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("用户创建时间")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.email)
                    .HasColumnType("varchar(32)")
                    .HasComment("email")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.enabled)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'")
                    .HasComment("0无效用户，1是有效用户");

                entity.Property(e => e.nickname)
                    .HasColumnType("varchar(64)")
                    .HasComment("昵称")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.org_id)
                    .HasColumnType("int(11)")
                    .HasComment("组织id");

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("密码")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.phone)
                    .HasColumnType("varchar(16)")
                    .HasComment("手机号")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.portrait)
                    .HasColumnType("varchar(255)")
                    .HasComment("头像图片路径")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.username)
                    .IsRequired()
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("用户名")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<sys_user_role>(entity =>
            {
                entity.HasComment("用户角色关系表");

                entity.Property(e => e.id).HasColumnType("int(11)");

                entity.Property(e => e.role_id)
                    .HasColumnType("int(11)")
                    .HasComment("角色自增id");

                entity.Property(e => e.user_id)
                    .HasColumnType("int(11)")
                    .HasComment("用户自增id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
