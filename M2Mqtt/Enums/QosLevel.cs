﻿namespace Tevux.Protocols.Mqtt {
    public enum QosLevel : byte {
        AtMostOnce = 0x00,
        AtLeastOnce = 0x01,
        ExactlyOnce = 0x02
    }
}
