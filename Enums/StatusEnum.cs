﻿namespace CrewBackend.Enums
{
    public enum StatusEnum
    {
        Ok = 0,
        NotFound = 1,
        ValidationError = 2,
        ResourceExist = 3,
        AuthenticationError = 4,
        Expired = 5,
        UnknownError = 1000,
    }
}
