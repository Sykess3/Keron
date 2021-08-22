using System;
using CodeBase.Services;

namespace CodeBase.Infrastructure.Services.Ads
{
    public interface IAdsService : IService
    {
        event Action RewardedVideoReady;
        bool IsRewardedVideoReady { get; }
        int RewardedVideo_Reward { get; }
        void Initialize();
        void ShowRewardedVideo(Action onVideoFinished);
    }
}