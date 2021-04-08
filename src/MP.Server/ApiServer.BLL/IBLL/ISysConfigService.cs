using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;

namespace ApiServer.BLL.IBLL
{
    public interface ISysConfigService
    {
        MsgModel GetSysConfigList();
        MsgModel QueryConfigs(string configLik);

        string GetConfigItem(string paramKey);
        MsgModel UpdateConfig(sys_config sys_Config);

        MsgModel AddConfig(sys_config sys_Config);

        MsgModel DeleteConfig(long configId);
    }
}
