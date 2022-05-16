{application, 'rabbitmq_stream_common', [
	{description, "RabbitMQ Stream Common"},
	{vsn, "3.10.1"},
	{id, "v3.10.1"},
	{modules, ['rabbit_stream_core']},
	{registered, []},
	{applications, [kernel,stdlib]},
	{env, [
]}
]}.