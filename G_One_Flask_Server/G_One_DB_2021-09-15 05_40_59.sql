-- --------------------------------------------------------
-- 호스트:                          gone.gvsolgryn.de
-- 서버 버전:                        10.3.31-MariaDB-0ubuntu0.20.04.1 - Ubuntu 20.04
-- 서버 OS:                        debian-linux-gnu
-- HeidiSQL 버전:                  11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- 테이블 데이터 G_One_DB.log:~100 rows (대략적) 내보내기
/*!40000 ALTER TABLE `log` DISABLE KEYS */;
INSERT INTO `log` (`log`, `use_program`, `sensor`, `def_location`, `control_time`, `sql_success`, `error_log`, `sql_run_time`) VALUES
	(1, 'flask_debug', 'LED', 'LED_ON', '2021-08-29 16:36:39', 'success', 'none', '0.07'),
	(2, 'flask_debug', 'LED', 'LED_OFF', '2021-08-31 00:21:46', 'success', 'none', '0.01'),
	(3, 'flask_debug', 'LED', 'LED_ON', '2021-08-31 00:36:00', 'success', 'none', '0.01'),
	(4, 'flask_debug', 'LED', 'LED_OFF', '2021-08-31 00:36:01', 'success', 'none', '0.01'),
	(5, 'flask_debug', 'MULTI', 'MULTI_ON', '2021-08-31 00:36:01', 'success', 'none', '0.01'),
	(6, 'flask_debug', 'MULTI', 'MULTI_OFF', '2021-08-31 00:36:02', 'success', 'none', '0.01'),
	(7, 'flask_debug', 'MULTI', 'MULTI_ON', '2021-08-31 00:37:06', 'success', 'none', '5.12'),
	(8, 'flask_debug', 'LED', 'LED_ON', '2021-08-31 00:37:06', 'success', 'none', '1.06'),
	(9, 'flask_debug', 'MULTI', 'MULTI_OFF', '2021-08-31 00:37:09', 'success', 'none', '0.01'),
	(10, 'flask_debug', 'MULTI', 'MULTI_ON', '2021-08-31 00:37:10', 'success', 'none', '0.08'),
	(11, 'flask_debug', 'MULTI', 'MULTI_OFF', '2021-08-31 00:37:12', 'success', 'none', '0.08'),
	(12, 'flask_debug', 'MULTI', 'MULTI_ON', '2021-08-31 00:37:13', 'success', 'none', '0.01'),
	(13, 'flask_debug', 'MULTI', 'MULTI_OFF', '2021-08-31 00:37:13', 'success', 'none', '0.01'),
	(14, 'flask_debug', 'LED', 'LED_OFF', '2021-08-31 19:16:18', 'success', 'none', '0.01'),
	(15, 'flask_debug', 'MULTI', 'MULTI_ON', '2021-09-15 00:54:41', 'success', 'none', '0.07'),
	(16, 'flask_debug', 'MULTI', 'MULTI_OFF', '2021-09-15 00:54:41', 'success', 'none', '0.0'),
	(17, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:30:35', 'success', 'none', '0.0'),
	(18, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 03:31:22', 'success', 'none', '0.01'),
	(19, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 03:37:42', 'success', 'none', '0.0'),
	(20, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 03:39:32', 'success', 'none', '0.0'),
	(21, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:52:15', 'success', 'none', '0.0'),
	(22, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:52:39', 'success', 'none', '0.0'),
	(23, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:53:21', 'success', 'none', '0.0'),
	(24, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:53:23', 'success', 'none', '0.0'),
	(25, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:53:24', 'success', 'none', '0.0'),
	(26, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:53:25', 'success', 'none', '0.01'),
	(27, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:53:26', 'success', 'none', '0.0'),
	(28, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:53:27', 'success', 'none', '0.0'),
	(29, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:56:57', 'success', 'none', '0.0'),
	(30, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:56:58', 'success', 'none', '0.0'),
	(31, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:56:59', 'success', 'none', '0.0'),
	(32, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:56:59', 'success', 'none', '0.0'),
	(33, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 03:57:00', 'success', 'none', '0.0'),
	(34, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 03:57:01', 'success', 'none', '0.0'),
	(35, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 03:57:02', 'success', 'none', '0.0'),
	(36, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 03:57:02', 'success', 'none', '0.0'),
	(37, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:57:03', 'success', 'none', '0.0'),
	(38, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:57:03', 'success', 'none', '0.0'),
	(39, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:57:06', 'success', 'none', '0.0'),
	(40, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:57:07', 'success', 'none', '0.0'),
	(41, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 03:57:36', 'success', 'none', '0.0'),
	(42, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 03:57:37', 'success', 'none', '0.0'),
	(43, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 03:57:40', 'success', 'none', '0.0'),
	(44, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 03:57:40', 'success', 'none', '0.0'),
	(45, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 03:57:43', 'success', 'none', '0.0'),
	(46, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 03:57:43', 'success', 'none', '0.0'),
	(47, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:57:44', 'success', 'none', '0.0'),
	(48, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:57:44', 'success', 'none', '0.0'),
	(49, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:57:45', 'success', 'none', '0.0'),
	(50, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:57:45', 'success', 'none', '0.0'),
	(51, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:57:46', 'success', 'none', '0.01'),
	(52, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:57:46', 'success', 'none', '0.0'),
	(53, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:58:30', 'success', 'none', '0.06'),
	(54, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:58:31', 'success', 'none', '0.0'),
	(55, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 03:58:32', 'success', 'none', '0.0'),
	(56, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 03:58:32', 'success', 'none', '0.0'),
	(57, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:58:33', 'success', 'none', '0.0'),
	(58, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:58:33', 'success', 'none', '0.0'),
	(59, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:58:33', 'success', 'none', '0.0'),
	(60, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:58:33', 'success', 'none', '0.0'),
	(61, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 03:58:34', 'success', 'none', '0.0'),
	(62, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 03:58:35', 'success', 'none', '0.0'),
	(63, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 03:58:36', 'success', 'none', '0.0'),
	(64, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 03:58:36', 'success', 'none', '0.0'),
	(65, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:02:37', 'success', 'none', '0.0'),
	(66, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 04:02:38', 'success', 'none', '0.01'),
	(67, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 04:02:39', 'success', 'none', '0.0'),
	(68, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 04:02:40', 'success', 'none', '0.0'),
	(69, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:43:57', 'fail', 'not enough arguments for format string', '0.01'),
	(70, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:44:05', 'fail', 'not enough arguments for format string', '0.0'),
	(71, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:44:42', 'fail', 'not enough arguments for format string', '0.0'),
	(72, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:44:48', 'fail', 'not enough arguments for format string', '0.0'),
	(73, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:44:48', 'fail', 'not enough arguments for format string', '0.0'),
	(74, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:44:49', 'fail', 'not enough arguments for format string', '0.0'),
	(75, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:45:44', 'fail', '(1054, "Unknown column \'led_status\' in \'field list\'")', '0.0'),
	(76, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:45:45', 'fail', '(1054, "Unknown column \'led_status\' in \'field list\'")', '0.0'),
	(77, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:45:45', 'fail', '(1054, "Unknown column \'led_status\' in \'field list\'")', '0.0'),
	(78, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:45:45', 'fail', '(1054, "Unknown column \'led_status\' in \'field list\'")', '0.0'),
	(79, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:45:45', 'fail', '(1054, "Unknown column \'led_status\' in \'field list\'")', '0.0'),
	(80, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:47:00', 'success', 'none', '0.0'),
	(81, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 04:50:19', 'success', 'none', '0.0'),
	(82, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 04:50:20', 'success', 'none', '0.0'),
	(83, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 04:50:21', 'success', 'none', '0.0'),
	(84, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 04:50:21', 'success', 'none', '0.0'),
	(85, 'flask_debug', 'Power_Strip', 'Power_Strip_ON', '2021-09-15 04:50:22', 'success', 'none', '0.0'),
	(86, 'flask_debug', 'Power_Strip', 'Power_Strip_OFF', '2021-09-15 04:50:23', 'success', 'none', '0.0'),
	(87, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 04:50:23', 'success', 'none', '0.0'),
	(88, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:50:41', 'success', 'none', '0.0'),
	(89, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 04:50:43', 'success', 'none', '0.0'),
	(90, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:50:47', 'success', 'none', '0.07'),
	(91, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 04:50:49', 'success', 'none', '0.0'),
	(92, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:53:51', 'success', 'none', '0.0'),
	(93, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 04:53:56', 'success', 'none', '0.0'),
	(94, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:54:14', 'success', 'none', '0.0'),
	(95, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 04:54:17', 'success', 'none', '0.01'),
	(96, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:54:18', 'success', 'none', '0.0'),
	(97, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 04:54:20', 'success', 'none', '0.0'),
	(98, 'flask_debug', 'LED', 'LED_ON', '2021-09-15 04:55:35', 'success', 'none', '0.0'),
	(99, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 04:55:37', 'success', 'none', '0.0'),
	(100, 'flask_debug', 'LED', 'LED_OFF', '2021-09-15 04:55:37', 'success', 'none', '0.0');
/*!40000 ALTER TABLE `log` ENABLE KEYS */;

-- 테이블 데이터 G_One_DB.sensor_status:~2 rows (대략적) 내보내기
/*!40000 ALTER TABLE `sensor_status` DISABLE KEYS */;
INSERT INTO `sensor_status` (`id`, `sensor`, `status`, `led_value`, `last_use`) VALUES
	(1, 'LED', 0, 0, '2021-09-15 04:55:37'),
	(2, 'Power_Strip', 0, 0, '2021-09-15 04:50:23');
/*!40000 ALTER TABLE `sensor_status` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
