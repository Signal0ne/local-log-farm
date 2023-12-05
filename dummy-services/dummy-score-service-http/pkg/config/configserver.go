package config

import (
	"fmt"

	"github.com/spf13/viper"
)

type ConfigServer struct {
	ServerPort    string `mapstructure:"SERVER_PORT"`
	DBUri         string `mapstructure:"DB_URI"`
	DDConnRetries int    `mapstructure:"DB_CONN_RETRY"`
}

func NewConfigServer(envName string) (cfs ConfigServer, err error) {
	viper.SetConfigName(fmt.Sprintf(".%s", envName))
	viper.AddConfigPath(".")
	viper.SetConfigType("env")
	err = viper.ReadInConfig()
	if err != nil {
		return
	}
	err = viper.Unmarshal(&cfs)
	return
}
