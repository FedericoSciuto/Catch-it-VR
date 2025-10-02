using System.Collections.Generic;

public static class UserAccounts
{
    public static readonly Dictionary<string, (string address, string privateKey)> accountMap = new()
    {
        { "Federico", ("0x9616001954c04F070e2fD72f7CeeC5F2C8767f76", "0xcfbbfcd96af03162dcd46a0f4be9daf691f0a34b7de1bbfbd39682bcb55e4a0b") }, // Account 2 Ganache
        { "Luca", ("0xF48d4bBeB4864E09f675225172BE731E57695603", "0x395961a0336fdd0cf48acc8293ee1bf02cb0053d766c82200372b5244775c26d") }, // Account 3 Ganache
        { "Antonino", ("0x6D153023e87Ee9308AD19A46F4fF737164330fe3", "0xcdafca646494563f80f1e2df12d3c677530abf4ea24dbea70ad4eb2bc84924e1") } // Account 4 Ganache
    };
}
