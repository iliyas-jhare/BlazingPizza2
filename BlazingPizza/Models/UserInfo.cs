﻿namespace BlazingPizza.Models;

public record UserInfo
{
    public bool IsAuthenticated { get; set; }

    public string Name { get; set; }
}
