namespace EB.Features.Shared.Model
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Resources;

    [Serializable]
    public class UserLevel : Enumeration
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Super = new UserLevel(4, "Super");

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Master = new UserLevel(3, "Master Agent");

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Agent = new UserLevel(2, "Agent");

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Member = new UserLevel(1, "Member");

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Headline = new UserLevel(0, "Headline");

        public UserLevel()
        {
        }

        private UserLevel(byte value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsAgentLevel() => this == Agent;

        public bool IsNotAgentLevel() => !IsAgentLevel();

        public bool IsMasterLevel() => this == Master;

        public bool IsNotMasterLevel() => !IsMasterLevel();

        public bool IsSuperLevel() => this == Super;

        public bool IsMasterOrSuperLevel() => IsMasterLevel() || IsSuperLevel();

        public bool IsNotSuperLevel() => !IsSuperLevel();

        public bool IsMemberLevel() => this == Member;

        public bool IsHeadlineLevel() => this == Headline;

        public bool IsNotHeadlineLevel() => !IsHeadlineLevel();

        public bool IsDirectUplineOf(byte downlineLevelValue)
            => GapBetweenTwoLevels(Value, downlineLevelValue) == 1;

        public bool IsDirectUplineOf(Enumeration downlineLevel)
            => (downlineLevel != null) && IsDirectUplineOf(downlineLevel.Value);

        public bool IsUplineOf(byte downlineLevelValue)
            => GapBetweenTwoLevels(Value, downlineLevelValue) >= 1;

        public bool IsUplineOf(Enumeration downlineLevel)
            => (downlineLevel != null) && IsUplineOf(downlineLevel.Value);

        public bool IsEqualOrUplineOf(Enumeration custLevel)
           => this.Value == custLevel.Value || IsUplineOf(custLevel.Value);

        public UserLevel GetDirectUplineLevel() => FromValue<UserLevel>((byte)(Value + 1));

        public UserLevel GetDirectDownLineLevel() => FromValue<UserLevel>((byte)(Value - 1));

        public bool IsDirectDownlineOf(Enumeration uplineLevel)
            => (uplineLevel != null) && GapBetweenTwoLevels(uplineLevel.Value, Value) == 1;

        public bool Is(Enumeration level)
            => (level != null) && this == level;

        public bool IsNot(Enumeration level) => !Is(level);

        private static int GapBetweenTwoLevels(byte uplineLevelValue, byte downlineLevelValue)
            => uplineLevelValue - downlineLevelValue;

        public string DisplayNameInMultipleLanguage
        {
            get
            {
                if (IsSuperLevel())
                {
                    return AppResources.Super;
                }

                if (IsMasterLevel())
                {
                    return AppResources.MasterAgent;
                }

                if (IsAgentLevel())
                {
                    return AppResources.Agent;
                }

                if (IsMemberLevel())
                {
                    return AppResources.Member;
                }

                return IsHeadlineLevel()
                    ? AppResources.Headline
                    : "Unknown";
            }
        }
    }
}
