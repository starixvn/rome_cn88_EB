namespace EB.Features.Shared.Model
{
    using System;

    [Serializable]
    public class UserLevel : Enumeration
    {
        public static readonly UserLevel Super = new UserLevel(4, "SMA");

        public static readonly UserLevel Master = new UserLevel(3, "MA");

        public static readonly UserLevel Agent = new UserLevel(2, "Agent");

        public static readonly UserLevel Member = new UserLevel(1, "Member");

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

        public bool IsMemberOrMasterLevel() => IsMemberLevel() || IsMasterLevel();

        public bool IsNotMemberLevel() => !IsMemberLevel();

        public bool IsHeadline() => this == Headline;

        public bool IsNotHeadline() => !IsHeadline();

        public bool IsDirectUplineOf(byte downlineLevelValue)
            => GapBetweenTwoLevels(Value, downlineLevelValue) == 1;

        public bool IsDirectUplineOf(Enumeration downlineLevel)
            => (downlineLevel != null) && IsDirectUplineOf(downlineLevel.Value);

        public bool IsUplineOf(byte downlineLevelValue)
            => GapBetweenTwoLevels(Value, downlineLevelValue) >= 1;

        public bool IsUplineOf(Enumeration downlineLevel)
            => (downlineLevel != null) && IsUplineOf(downlineLevel.Value);

        public bool IsEqualOrUplineOf(Enumeration custLevel)
           => Value == custLevel.Value || IsUplineOf(custLevel.Value);

        public UserLevel GetDirectDownLineLevel() => FromValue<UserLevel>((byte)(Value - 1));

        public UserLevel GetDirectUpLineLevel() => IsSuperLevel() ? Headline : FromValue<UserLevel>((byte)(Value + 1));

        public bool IsDirectDownlineOf(Enumeration uplineLevel)
            => (uplineLevel != null) && GapBetweenTwoLevels(uplineLevel.Value, Value) == 1;

        public bool Is(Enumeration level)
            => (level != null) && this == level;

        public bool IsNot(Enumeration level) => !Is(level);

        public bool IsDownlineOf(byte uplineLevelValue)
            => GapBetweenTwoLevels(Value, uplineLevelValue) <= 1;

        public bool IsDownlineOf(Enumeration uplineLevel)
            => (uplineLevel != null) && IsDownlineOf(uplineLevel.Value);

        private static int GapBetweenTwoLevels(byte uplineLevelValue, byte downlineLevelValue)
            => uplineLevelValue - downlineLevelValue;
    }
}
