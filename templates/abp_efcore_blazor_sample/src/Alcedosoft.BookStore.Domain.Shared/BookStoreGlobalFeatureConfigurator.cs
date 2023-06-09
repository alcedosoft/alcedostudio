﻿namespace Alcedosoft.BookStore;

public static class BookStoreGlobalFeatureConfigurator
{
    private static readonly OneTimeRunner _oneTimeRunner = new();

    public static void Configure()
    {
        _oneTimeRunner.Run(() =>
        {
            /* You can configure (enable/disable) global features of the used modules here.
             *
             * YOU CAN SAFELY DELETE THIS CLASS AND REMOVE ITS USAGES IF YOU DON'T NEED TO IT!
             *
             * Please refer to the documentation to lear more about the Global Features System:
             * https://docs.abp.io/en/abp/latest/Global-Features
             */
        });
    }
}
